using System;

namespace QuantityMeasurementApp.Models.Entities
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        public Guid Id { get; }
        public string Description { get; }
        public bool IsError { get; }
        public string ErrorMessage { get; }
        public DateTime CreatedAt { get; }

        public QuantityMeasurementEntity(string description)
        {
            Id = Guid.NewGuid();
            Description = description;
            IsError = false;
            ErrorMessage = string.Empty;
            CreatedAt = DateTime.UtcNow;
        }

        public QuantityMeasurementEntity(string description, string errorMessage)
        {
            Id = Guid.NewGuid();
            Description = description;
            IsError = true;
            ErrorMessage = errorMessage;
            CreatedAt = DateTime.UtcNow;
        }

        private QuantityMeasurementEntity(
            Guid id,
            string description,
            bool isError,
            string errorMessage,
            DateTime createdAt
        )
        {
            Id = id;
            Description = description;
            IsError = isError;
            ErrorMessage = errorMessage;
            CreatedAt = createdAt;
        }

        public static QuantityMeasurementEntity Rehydrate(
            Guid id,
            string description,
            bool isError,
            string errorMessage,
            DateTime createdAt
        )
        {
            return new QuantityMeasurementEntity(id, description, isError, errorMessage, createdAt);
        }
    }
}
