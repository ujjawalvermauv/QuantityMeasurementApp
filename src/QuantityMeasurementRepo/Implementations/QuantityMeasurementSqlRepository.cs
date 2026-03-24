using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using QuantityMeasurementApp.QuantityMeasurementModel;
using QuantityMeasurementApp.QuantityMeasurementRepo.Interfaces;
using QuantityMeasurementApp.QuantityMeasurementRepo.Models;

namespace QuantityMeasurementApp.QuantityMeasurementRepo.Implementations
{
    /// <summary>
    /// SQL-backed repository for quantity measurement operation history.
    /// </summary>
    public class QuantityMeasurementSqlRepository : IQuantityMeasurementRepository
    {
        private readonly string _connectionString;

        public QuantityMeasurementSqlRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            const string sql = @"
INSERT INTO dbo.QuantityMeasurementHistory
(
    Operation,
    Operand1Value, Operand1UnitName, Operand1MeasurementType,
    Operand2Value, Operand2UnitName, Operand2MeasurementType,
    ResultValue, ResultUnitName, ResultMeasurementType,
    ErrorMessage, CreatedAtUtc
)
VALUES
(
    @Operation,
    @Operand1Value, @Operand1UnitName, @Operand1MeasurementType,
    @Operand2Value, @Operand2UnitName, @Operand2MeasurementType,
    @ResultValue, @ResultUnitName, @ResultMeasurementType,
    @ErrorMessage, @CreatedAtUtc
);";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Operation", entity.Operation);

            command.Parameters.AddWithValue("@Operand1Value", entity.Operand1.Value);
            command.Parameters.AddWithValue("@Operand1UnitName", entity.Operand1.UnitName);
            command.Parameters.AddWithValue("@Operand1MeasurementType", entity.Operand1.MeasurementType);

            command.Parameters.AddWithValue("@Operand2Value", GetDbValue(entity.Operand2?.Value));
            command.Parameters.AddWithValue("@Operand2UnitName", GetDbValue(entity.Operand2?.UnitName));
            command.Parameters.AddWithValue("@Operand2MeasurementType", GetDbValue(entity.Operand2?.MeasurementType));

            command.Parameters.AddWithValue("@ResultValue", GetDbValue(entity.Result?.Value));
            command.Parameters.AddWithValue("@ResultUnitName", GetDbValue(entity.Result?.UnitName));
            command.Parameters.AddWithValue("@ResultMeasurementType", GetDbValue(entity.Result?.MeasurementType));

            command.Parameters.AddWithValue("@ErrorMessage", GetDbValue(entity.ErrorMessage));
            command.Parameters.AddWithValue("@CreatedAtUtc", entity.Timestamp.ToUniversalTime());

            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            const string sql = @"
SELECT
    Operation,
    Operand1Value, Operand1UnitName, Operand1MeasurementType,
    Operand2Value, Operand2UnitName, Operand2MeasurementType,
    ResultValue, ResultUnitName, ResultMeasurementType,
    ErrorMessage, CreatedAtUtc
FROM dbo.QuantityMeasurementHistory
ORDER BY Id DESC;";

            var measurements = new List<QuantityMeasurementEntity>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var operation = reader.GetString(reader.GetOrdinal("Operation"));
                var timestampUtc = reader.GetDateTime(reader.GetOrdinal("CreatedAtUtc"));
                var errorMessage = GetNullableString(reader, "ErrorMessage");

                var operand1 = new QuantityDTO
                {
                    Value = reader.GetDouble(reader.GetOrdinal("Operand1Value")),
                    UnitName = reader.GetString(reader.GetOrdinal("Operand1UnitName")),
                    MeasurementType = reader.GetString(reader.GetOrdinal("Operand1MeasurementType"))
                };

                QuantityDTO? operand2 = null;
                if (!reader.IsDBNull(reader.GetOrdinal("Operand2Value"))
                    && !reader.IsDBNull(reader.GetOrdinal("Operand2UnitName"))
                    && !reader.IsDBNull(reader.GetOrdinal("Operand2MeasurementType")))
                {
                    operand2 = new QuantityDTO
                    {
                        Value = reader.GetDouble(reader.GetOrdinal("Operand2Value")),
                        UnitName = reader.GetString(reader.GetOrdinal("Operand2UnitName")),
                        MeasurementType = reader.GetString(reader.GetOrdinal("Operand2MeasurementType"))
                    };
                }

                QuantityDTO? result = null;
                if (!reader.IsDBNull(reader.GetOrdinal("ResultValue"))
                    && !reader.IsDBNull(reader.GetOrdinal("ResultUnitName"))
                    && !reader.IsDBNull(reader.GetOrdinal("ResultMeasurementType")))
                {
                    result = new QuantityDTO
                    {
                        Value = reader.GetDouble(reader.GetOrdinal("ResultValue")),
                        UnitName = reader.GetString(reader.GetOrdinal("ResultUnitName")),
                        MeasurementType = reader.GetString(reader.GetOrdinal("ResultMeasurementType"))
                    };
                }

                var localTimestamp = DateTime.SpecifyKind(timestampUtc, DateTimeKind.Utc).ToLocalTime();
                QuantityMeasurementEntity entity;
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    entity = operand2 == null
                        ? new QuantityMeasurementEntity(operand1, operation, errorMessage, localTimestamp)
                        : new QuantityMeasurementEntity(operand1, operand2, operation, errorMessage, localTimestamp);
                }
                else
                {
                    entity = operand2 == null
                        ? new QuantityMeasurementEntity(operand1, operation, result, localTimestamp)
                        : new QuantityMeasurementEntity(operand1, operand2, operation, result, localTimestamp);
                }

                measurements.Add(entity);
            }

            return measurements;
        }

        private static object GetDbValue(object? value)
        {
            return value ?? DBNull.Value;
        }

        private static string? GetNullableString(SqlDataReader reader, string column)
        {
            var ordinal = reader.GetOrdinal(column);
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }
    }
}