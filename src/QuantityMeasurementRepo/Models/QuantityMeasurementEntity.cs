using System;
using QuantityMeasurementApp.QuantityMeasurementModel;

namespace QuantityMeasurementApp.QuantityMeasurementRepo.Models
{
    /// <summary>
    /// The QuantityMeasurementEntity is designed to be a comprehensive data holder for all aspects of a quantity measurement operation,
    /// including the operands, the operation type, and the result. It also includes fields for error handling.
    /// This class is designed to be Immutable, initialized through constructors.
    /// </summary>
    [Serializable]
    public class QuantityMeasurementEntity
    {
        /// <summary>
        /// The first operand.
        /// </summary>
        public QuantityDTO Operand1 { get; private set; }

        /// <summary>
        /// The second operand (null for single operand operations).
        /// </summary>
        public QuantityDTO? Operand2 { get; private set; }

        /// <summary>
        /// The operation type (e.g., "COMPARE", "CONVERT", "ADD").
        /// </summary>
        public string Operation { get; private set; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public QuantityDTO? Result { get; private set; }

        /// <summary>
        /// Error message if the operation failed.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Timestamp of the operation.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Constructor for single operand operations (e.g., conversion).
        /// </summary>
        /// <param name="operand1">The operand</param>
        /// <param name="operation">The operation type</param>
        /// <param name="result">The result</param>
        /// <param name="timestamp">Optional timestamp for persisted history records</param>
        public QuantityMeasurementEntity(QuantityDTO operand1, string operation, QuantityDTO? result, DateTime? timestamp = null)
        {
            Operand1 = operand1;
            Operand2 = null;
            Operation = operation;
            Result = result;
            ErrorMessage = null;
            Timestamp = timestamp ?? DateTime.Now;
        }

        /// <summary>
        /// Constructor for binary operations (e.g., add, subtract).
        /// </summary>
        /// <param name="operand1">The first operand</param>
        /// <param name="operand2">The second operand</param>
        /// <param name="operation">The operation type</param>
        /// <param name="result">The result</param>
        /// <param name="timestamp">Optional timestamp for persisted history records</param>
        public QuantityMeasurementEntity(QuantityDTO operand1, QuantityDTO? operand2, string operation, QuantityDTO? result, DateTime? timestamp = null)
        {
            Operand1 = operand1;
            Operand2 = operand2;
            Operation = operation;
            Result = result;
            ErrorMessage = null;
            Timestamp = timestamp ?? DateTime.Now;
        }

        /// <summary>
        /// Constructor for error cases.
        /// </summary>
        /// <param name="operand1">The first operand</param>
        /// <param name="operand2">The second operand (optional)</param>
        /// <param name="operation">The operation type</param>
        /// <param name="errorMessage">The error message</param>
        /// <param name="timestamp">Optional timestamp for persisted history records</param>
        public QuantityMeasurementEntity(QuantityDTO operand1, QuantityDTO? operand2, string operation, string? errorMessage, DateTime? timestamp = null)
        {
            Operand1 = operand1;
            Operand2 = operand2;
            Operation = operation;
            Result = null;
            ErrorMessage = errorMessage;
            Timestamp = timestamp ?? DateTime.Now;
        }

        /// <summary>
        /// Constructor for single operand error.
        /// </summary>
        /// <param name="operand1">The operand</param>
        /// <param name="operation">The operation type</param>
        /// <param name="errorMessage">The error message</param>
        /// <param name="timestamp">Optional timestamp for persisted history records</param>
        public QuantityMeasurementEntity(QuantityDTO operand1, string operation, string? errorMessage, DateTime? timestamp = null)
        {
            Operand1 = operand1;
            Operand2 = null;
            Operation = operation;
            Result = null;
            ErrorMessage = errorMessage;
            Timestamp = timestamp ?? DateTime.Now;
        }

        /// <summary>
        /// Returns a string representation of the entity.
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"Operation: {Operation}, Error: {ErrorMessage}";
            }
            else
            {
                string operands = Operand2 != null ?
                    $"{Operand1.Value} {Operand1.UnitName} and {Operand2.Value} {Operand2.UnitName}" :
                    $"{Operand1.Value} {Operand1.UnitName}";
                if (Result == null)
                {
                    return $"Operation: {Operation}, Operands: {operands}, Result: N/A";
                }

                return $"Operation: {Operation}, Operands: {operands}, Result: {Result.Value} {Result.UnitName}";
            }
        }
    }
}
