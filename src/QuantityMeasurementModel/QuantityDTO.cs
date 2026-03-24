using System;

namespace QuantityMeasurementApp.QuantityMeasurementModel
{
    /// <summary>
    /// Data Transfer Object (DTO) for holding quantity measurement input data - value and corresponding unit and its measurement.
    /// </summary>
    public class QuantityDTO
    {
        /// <summary>
        /// The quantity value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// The unit name (e.g., "FEET", "KILOGRAM").
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// The measurement type (e.g., "Length", "Weight").
        /// </summary>
        public string MeasurementType { get; set; } = string.Empty;
    }
}