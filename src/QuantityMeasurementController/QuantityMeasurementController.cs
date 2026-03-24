using System;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Interfaces;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Exceptions;
using QuantityMeasurementApp.QuantityMeasurementModel;
using QuantityMeasurementApp.QuantityMeasurementRepo.Models;

namespace QuantityMeasurementApp.QuantityMeasurementController
{
    /// <summary>
    /// QuantityMeasurementController serves as the entry point for the QuantityMeasurementApp.
    /// This controller is responsible for handling requests related to quantity measurements,
    /// including comparison, conversion, and arithmetic operations on various units of measurement.
    /// </summary>
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _service;

        /// <summary>
        /// Initializes a new instance of QuantityMeasurementController.
        /// </summary>
        /// <param name="service">The quantity measurement service</param>
        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        /// <summary>
        /// Compares two quantities for equality.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>True if equal, false otherwise</returns>
        public bool Compare(QuantityDTO dto1, QuantityDTO dto2)
        {
            var result = _service.Compare(dto1, dto2);
            return result.Value == 1;
        }

        /// <summary>
        /// Converts a quantity to another unit.
        /// </summary>
        /// <param name="dto">Quantity DTO to convert</param>
        /// <param name="targetUnitName">Target unit name</param>
        /// <returns>QuantityDTO with converted value</returns>
        public QuantityDTO Convert(QuantityDTO dto, string targetUnitName)
        {
            return _service.Convert(dto, targetUnitName);
        }

        /// <summary>
        /// Adds two quantities.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>QuantityDTO with sum</returns>
        public QuantityDTO Add(QuantityDTO dto1, QuantityDTO dto2)
        {
            return _service.Add(dto1, dto2);
        }

        /// <summary>
        /// Subtracts two quantities.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>QuantityDTO with difference</returns>
        public QuantityDTO Subtract(QuantityDTO dto1, QuantityDTO dto2)
        {
            return _service.Subtract(dto1, dto2);
        }

        /// <summary>
        /// Divides two quantities.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>QuantityDTO with quotient</returns>
        public QuantityDTO Divide(QuantityDTO dto1, QuantityDTO dto2)
        {
            return _service.Divide(dto1, dto2);
        }

        /// <summary>
        /// Returns all persisted operation history records.
        /// </summary>
        /// <returns>List of history entities</returns>
        public List<QuantityMeasurementEntity> GetOperationHistory()
        {
            return _service.GetOperationHistory();
        }
    }
}