using System;

namespace QuantityMeasurementApp.Models.Entities
{
    [Serializable]
    public class UserAccountEntity
    {
        public Guid Id { get; }
        public string FullName { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public DateTime CreatedAtUtc { get; }

        public UserAccountEntity(string fullName, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAtUtc = DateTime.UtcNow;
        }

        private UserAccountEntity(
            Guid id,
            string fullName,
            string email,
            string passwordHash,
            DateTime createdAtUtc)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAtUtc = createdAtUtc;
        }

        public static UserAccountEntity Rehydrate(
            Guid id,
            string fullName,
            string email,
            string passwordHash,
            DateTime createdAtUtc)
        {
            return new UserAccountEntity(id, fullName, email, passwordHash, createdAtUtc);
        }
    }
}
