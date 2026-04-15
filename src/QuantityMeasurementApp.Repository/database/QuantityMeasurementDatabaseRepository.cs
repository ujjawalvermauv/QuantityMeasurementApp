using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Data.SqlClient;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public sealed class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private const string HistoryTableName = "dbo.QuantityMeasurementHistoryEntries";
        private const int CommandTimeoutSeconds = 30;
        private const int MaxRetryAttempts = 3;
        private readonly string _connectionString;

        public QuantityMeasurementDatabaseRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(
                    "Connection string cannot be empty.",
                    nameof(connectionString)
                );
            }

            _connectionString = connectionString;
            EnsureSchema();
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            const string sql =
                @"
                INSERT INTO dbo.QuantityMeasurementHistoryEntries
                    (UserScope, Type, Operation, Input, Result, IsError, ErrorMessage, CreatedAt)
                VALUES
                    (@UserScope, @Type, @Operation, @Input, @Result, @IsError, @ErrorMessage, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.CommandTimeout = CommandTimeoutSeconds;

            command.Parameters.AddWithValue("@UserScope", entity.UserScope);
            command.Parameters.AddWithValue("@Type", entity.Type);
            command.Parameters.AddWithValue("@Operation", entity.Operation);
            command.Parameters.AddWithValue("@Input", entity.Input);
            command.Parameters.AddWithValue("@Result", entity.Result);
            command.Parameters.AddWithValue("@IsError", entity.IsError);
            command.Parameters.AddWithValue("@ErrorMessage", entity.ErrorMessage);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            OpenWithRetry(connection);
            var generatedId = Convert.ToInt32(ExecuteScalarWithRetry(command));
            entity.AssignId(generatedId);
        }

        public IEnumerable<QuantityMeasurementEntity> GetAll()
        {
            const string sql =
                @"
                SELECT Id, UserScope, Type, Operation, Input, Result, IsError, ErrorMessage, CreatedAt
                FROM dbo.QuantityMeasurementHistoryEntries
                ORDER BY CreatedAt DESC;";

            var entities = new List<QuantityMeasurementEntity>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.CommandTimeout = CommandTimeoutSeconds;

            OpenWithRetry(connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var userScope = reader.IsDBNull(1) ? "GUEST" : reader.GetString(1);
                var type = reader.IsDBNull(2) ? "Unknown" : reader.GetString(2);
                var operation = reader.IsDBNull(3) ? "Unknown" : reader.GetString(3);
                var input = reader.IsDBNull(4) ? "-" : reader.GetString(4);
                var result = reader.IsDBNull(5) ? "-" : reader.GetString(5);
                var isError = reader.GetBoolean(6);
                var errorMessage = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                var createdAt = reader.GetDateTime(8);

                var entity = QuantityMeasurementEntity.Rehydrate(
                    id,
                    userScope,
                    type,
                    operation,
                    input,
                    result,
                    isError,
                    errorMessage,
                    createdAt
                );

                entities.Add(entity);
            }

            return entities;
        }

        private void EnsureSchema()
        {
            const string sql =
                @"
                IF OBJECT_ID('dbo.QuantityMeasurementHistoryEntries', 'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.QuantityMeasurementHistoryEntries
                    (
                        Id INT IDENTITY(1001,1) NOT NULL PRIMARY KEY,
                        UserScope NVARCHAR(128) NOT NULL,
                        Type NVARCHAR(64) NOT NULL,
                        Operation NVARCHAR(64) NOT NULL,
                        Input NVARCHAR(512) NOT NULL,
                        Result NVARCHAR(512) NOT NULL,
                        IsError BIT NOT NULL,
                        ErrorMessage NVARCHAR(1000) NOT NULL,
                        CreatedAt DATETIME2 NOT NULL
                    );
                    CREATE INDEX IX_QuantityMeasurementHistoryEntries_CreatedAt
                        ON dbo.QuantityMeasurementHistoryEntries(CreatedAt DESC);
                    CREATE INDEX IX_QuantityMeasurementHistoryEntries_UserScope
                        ON dbo.QuantityMeasurementHistoryEntries(UserScope);
                END;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.CommandTimeout = CommandTimeoutSeconds;

            OpenWithRetry(connection);
            ExecuteNonQueryWithRetry(command);
        }

        private static bool IsTransient(SqlException exception)
        {
            return exception.Number is -2 or 40613 or 40197 or 40501 or 10928 or 10929;
        }

        private static void OpenWithRetry(SqlConnection connection)
        {
            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    connection.Open();
                    return;
                }
                catch (SqlException ex) when (IsTransient(ex) && attempt < MaxRetryAttempts)
                {
                    Thread.Sleep(300 * attempt);
                }
            }
        }

        private static object? ExecuteScalarWithRetry(SqlCommand command)
        {
            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    return command.ExecuteScalar();
                }
                catch (SqlException ex) when (IsTransient(ex) && attempt < MaxRetryAttempts)
                {
                    Thread.Sleep(300 * attempt);
                }
            }
        }

        private static void ExecuteNonQueryWithRetry(SqlCommand command)
        {
            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    command.ExecuteNonQuery();
                    return;
                }
                catch (SqlException ex) when (IsTransient(ex) && attempt < MaxRetryAttempts)
                {
                    Thread.Sleep(300 * attempt);
                }
            }
        }
    }
}
