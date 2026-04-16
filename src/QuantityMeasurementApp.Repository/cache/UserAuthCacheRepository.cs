using System;
using System.Collections.Concurrent;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public sealed class UserAuthCacheRepository : IUserAuthRepository
    {
        private static readonly UserAuthCacheRepository _instance = new();
        private readonly ConcurrentDictionary<string, UserAccountEntity> _users =
            new(StringComparer.OrdinalIgnoreCase);

        public static UserAuthCacheRepository Instance => _instance;

        private UserAuthCacheRepository() { }

        public UserAccountEntity? GetByEmail(string email)
        {
            var normalized = NormalizeEmail(email);
            return _users.TryGetValue(normalized, out var user) ? user : null;
        }

        public UserAccountEntity Create(UserAccountEntity user)
        {
            ArgumentNullException.ThrowIfNull(user);
            var normalized = NormalizeEmail(user.Email);

            if (!_users.TryAdd(normalized, user))
            {
                throw new ArgumentException("A user with this email already exists.");
            }

            return user;
        }

        private static string NormalizeEmail(string email)
        {
            return (email ?? string.Empty).Trim().ToLowerInvariant();
        }
    }

    public sealed class UserAuthResilientRepository : IUserAuthRepository
    {
        private readonly IUserAuthRepository _primary;
        private readonly IUserAuthRepository _fallback;
        private readonly ILogger<UserAuthResilientRepository> _logger;

        public UserAuthResilientRepository(
            IUserAuthRepository primary,
            IUserAuthRepository fallback,
            ILogger<UserAuthResilientRepository> logger)
        {
            _primary = primary;
            _fallback = fallback;
            _logger = logger;
        }

        public UserAccountEntity? GetByEmail(string email)
        {
            try
            {
                return _primary.GetByEmail(email);
            }
            catch (SqlException ex)
            {
                _logger.LogWarning(ex, "Primary auth database unavailable. Reading user from in-memory fallback.");
                return _fallback.GetByEmail(email);
            }
        }

        public UserAccountEntity Create(UserAccountEntity user)
        {
            try
            {
                return _primary.Create(user);
            }
            catch (SqlException ex)
            {
                _logger.LogWarning(ex, "Primary auth database unavailable. Creating user in in-memory fallback.");
                return _fallback.Create(user);
            }
        }
    }
}