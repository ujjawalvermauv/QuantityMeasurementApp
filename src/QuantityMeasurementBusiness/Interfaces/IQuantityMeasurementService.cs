using QuantityMeasurementApp.QuantityMeasurementModel;

namespace QuantityMeasurementApp.QuantityMeasurementBusiness.Interfaces
{
    /// <summary>
    /// IQuantityMeasurementService interface provides contract methods for performing quantity measurement operations,
    /// including conversion, comparison, arithmetic operations, and division.
    /// </summary>
    public interface IQuantityMeasurementService
    {
        /// <summary>
        /// Compares two quantities.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>Result DTO with comparison result</returns>
        QuantityDTO Compare(QuantityDTO dto1, QuantityDTO dto2);

        /// <summary>
        /// Converts a quantity to another unit.
        /// </summary>
        /// <param name="dto">Quantity DTO to convert</param>
        /// <param name="targetUnitName">Target unit name</param>
        /// <returns>Result DTO with converted quantity</returns>
        QuantityDTO Convert(QuantityDTO dto, string targetUnitName);

        /// <summary>
        /// Adds two quantities.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>Result DTO with sum</returns>
        QuantityDTO Add(QuantityDTO dto1, QuantityDTO dto2);

        /// <summary>
        /// Subtracts the second quantity from the first.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>Result DTO with difference</returns>
        QuantityDTO Subtract(QuantityDTO dto1, QuantityDTO dto2);

        /// <summary>
        /// Divides the first quantity by the second.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>Result DTO with quotient</returns>
        QuantityDTO Divide(QuantityDTO dto1, QuantityDTO dto2);
    }
}