using System;

namespace QuantityMeasurementApp.Models.Entities
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        public int Id { get; private set; }
        public string UserScope { get; }
        public string Type { get; }
        public string Operation { get; }
        public string Input { get; }
        public string Result { get; }
        public bool IsError { get; }
        public string ErrorMessage { get; }
        public DateTime CreatedAt { get; }

        public QuantityMeasurementEntity(
            string type,
            string operation,
            string input,
            string result,
            string userScope = "GUEST"
        )
        {
            Id = 0;
            UserScope = string.IsNullOrWhiteSpace(userScope) ? "GUEST" : userScope;
            Type = string.IsNullOrWhiteSpace(type) ? "Unknown" : type;
            Operation = string.IsNullOrWhiteSpace(operation) ? "Unknown" : operation;
            Input = string.IsNullOrWhiteSpace(input) ? "-" : input;
            Result = string.IsNullOrWhiteSpace(result) ? "-" : result;
            IsError = false;
            ErrorMessage = string.Empty;
            CreatedAt = DateTime.UtcNow;
        }

        public QuantityMeasurementEntity(
            string type,
            string operation,
            string input,
            string result,
            string errorMessage,
            string userScope = "GUEST"
        )
        {
            Id = 0;
            UserScope = string.IsNullOrWhiteSpace(userScope) ? "GUEST" : userScope;
            Type = string.IsNullOrWhiteSpace(type) ? "Unknown" : type;
            Operation = string.IsNullOrWhiteSpace(operation) ? "Unknown" : operation;
            Input = string.IsNullOrWhiteSpace(input) ? "-" : input;
            Result = string.IsNullOrWhiteSpace(result) ? "-" : result;
            IsError = true;
            ErrorMessage = string.IsNullOrWhiteSpace(errorMessage) ? "Operation failed." : errorMessage;
            CreatedAt = DateTime.UtcNow;
        }

        private QuantityMeasurementEntity(
            int id,
            string userScope,
            string type,
            string operation,
            string input,
            string result,
            bool isError,
            string errorMessage,
            DateTime createdAt
        )
        {
            Id = id;
            UserScope = userScope;
            Type = type;
            Operation = operation;
            Input = input;
            Result = result;
            IsError = isError;
            ErrorMessage = errorMessage;
            CreatedAt = createdAt;
        }

        public void AssignId(int id)
        {
            if (id > 0)
            {
                Id = id;
            }
        }

        public static QuantityMeasurementEntity Rehydrate(
            int id,
            string userScope,
            string type,
            string operation,
            string input,
            string result,
            bool isError,
            string errorMessage,
            DateTime createdAt
        )
        {
            return new QuantityMeasurementEntity(
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
        }
    }
}
