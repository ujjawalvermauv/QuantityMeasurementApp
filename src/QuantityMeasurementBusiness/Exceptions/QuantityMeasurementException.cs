using System;

namespace QuantityMeasurementApp.QuantityMeasurementBusiness.Exceptions
{
    /// <summary>
    /// Custom exception class used to handle errors and exceptional conditions related to quantity measurement operations.
    /// </summary>
    public class QuantityMeasurementException : Exception
    {
        /// <summary>
        /// Initializes a new instance of QuantityMeasurementException with a specified error message.
        /// </summary>
        /// <param name="message">The error message</param>
        public QuantityMeasurementException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of QuantityMeasurementException with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">The inner exception</param>
        public QuantityMeasurementException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}