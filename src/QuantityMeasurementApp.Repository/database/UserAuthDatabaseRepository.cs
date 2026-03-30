using System;
using Microsoft.Data.SqlClient;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public sealed class UserAuthDatabaseRepository : IUserAuthRepository
    {
        private readonly string _connectionString;

        public UserAuthDatabaseRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be empty.", nameof(connectionString));
            }

            _connectionString = connectionString;
            EnsureSchema();
        }

        public UserAccountEntity? GetByEmail(string email)
        {
            const string sql = @"
                SELECT TOP 1 Id, FullName, Email, PasswordHash, CreatedAtUtc
                FROM dbo.Users
                WHERE Email = @Email;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email.Trim().ToLowerInvariant());

            connection.Open();
            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            var id = reader.GetGuid(0);
            var fullName = reader.GetString(1);
            var normalizedEmail = reader.GetString(2);
            var passwordHash = reader.GetString(3);
            var createdAtUtc = reader.GetDateTime(4);

            return UserAccountEntity.Rehydrate(id, fullName, normalizedEmail, passwordHash, createdAtUtc);
        }

        public UserAccountEntity Create(UserAccountEntity user)
        {
            ArgumentNullException.ThrowIfNull(user);

            const string sql = @"
                INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, CreatedAtUtc)
                VALUES (@Id, @FullName, @Email, @PasswordHash, @CreatedAtUtc);";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@FullName", user.FullName);
            command.Parameters.AddWithValue("@Email", user.Email.Trim().ToLowerInvariant());
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@CreatedAtUtc", user.CreatedAtUtc);

            connection.Open();
            command.ExecuteNonQuery();

            return user;
        }

        private void EnsureSchema()
        {
            const string sql = @"
                IF OBJECT_ID('dbo.Users', 'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.Users
                    (
                        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                        FullName NVARCHAR(200) NOT NULL,
                        Email NVARCHAR(320) NOT NULL,
                        PasswordHash NVARCHAR(512) NOT NULL,
                        CreatedAtUtc DATETIME2 NOT NULL
                    );

                    CREATE UNIQUE INDEX IX_Users_Email ON dbo.Users(Email);
                END;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
