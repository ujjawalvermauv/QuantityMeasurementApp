using System;

namespace QuantityMeasurementApp.QuantityMeasurementModel
{
    /// <summary>
    /// A generic model class for representing a quantity with its associated unit of measurement.
    /// Used internally within the service layer for performing operations on quantities.
    /// </summary>
    public class QuantityModel
    {
        /// <summary>
        /// The quantity value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// The measurement unit.
        /// </summary>
        public object Unit { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of QuantityModel.
        /// </summary>
        /// <param name="value">The quantity value</param>
        /// <param name="unit">The measurement unit</param>
        public QuantityModel(double value, Enum unit)
        {
            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuantityModel() { }
    }
}