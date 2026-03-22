using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public sealed class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
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
                INSERT INTO dbo.QuantityMeasurementOperations
                    (Id, Description, IsError, ErrorMessage, CreatedAt)
                VALUES
                    (@Id, @Description, @IsError, @ErrorMessage, @CreatedAt);";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Description", entity.Description);
            command.Parameters.AddWithValue("@IsError", entity.IsError);
            command.Parameters.AddWithValue("@ErrorMessage", entity.ErrorMessage);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public IEnumerable<QuantityMeasurementEntity> GetAll()
        {
            const string sql =
                @"
                SELECT Id, Description, IsError, ErrorMessage, CreatedAt
                FROM dbo.QuantityMeasurementOperations
                ORDER BY CreatedAt DESC;";

            var entities = new List<QuantityMeasurementEntity>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var id = reader.GetGuid(0);
                var description = reader.GetString(1);
                var isError = reader.GetBoolean(2);
                var errorMessage = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                var createdAt = reader.GetDateTime(4);

                var entity = QuantityMeasurementEntity.Rehydrate(
                    id,
                    description,
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
                IF OBJECT_ID('dbo.QuantityMeasurementOperations', 'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.QuantityMeasurementOperations
                    (
                        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                        Description NVARCHAR(1000) NOT NULL,
                        IsError BIT NOT NULL,
                        ErrorMessage NVARCHAR(1000) NOT NULL,
                        CreatedAt DATETIME2 NOT NULL
                    );
                    CREATE INDEX IX_QuantityMeasurementOperations_CreatedAt
                        ON dbo.QuantityMeasurementOperations(CreatedAt DESC);
                END;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
