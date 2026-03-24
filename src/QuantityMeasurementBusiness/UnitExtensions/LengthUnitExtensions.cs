namespace QuantityMeasurementApp.QuantityMeasurementBusiness.Units
{
    /// <summary>
    /// Extension methods for LengthUnit conversion behavior.
    /// Provides consistent conversion behavior across all length units.
    /// </summary>
    public static class LengthUnitExtensions
    {
        /// <summary>
        /// Conversion factors relative to base unit (INCHES).
        /// </summary>
        private static readonly Dictionary<LengthUnit, double> ConversionFactors = new()
        {
            { LengthUnit.FEET, 12.0 },
            { LengthUnit.INCHES, 1.0 },
            { LengthUnit.YARDS, 36.0 },
            { LengthUnit.CENTIMETERS, 0.393701 },
            { LengthUnit.MILLIMETER, 0.0393701 }
        };

        /// <summary>
        /// Gets conversion factor for the unit relative to base unit (INCHES).
        /// </summary>
        public static double GetConversionFactor(this LengthUnit unit)
        {
            if (ConversionFactors.TryGetValue(unit, out var factor))
                return factor;
            throw new ArgumentException("Invalid unit");
        }

        /// <summary>
        /// Converts a value from this unit to the base unit (INCHES).
        /// </summary>
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Converts a value from the base unit (INCHES) to this unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        /// <summary>
        /// Returns the readable unit name.
        /// </summary>
        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString();
        }

        /// <summary>
        /// Returns the measurement type.
        /// </summary>
        public static string GetMeasurementType(this LengthUnit unit)
        {
            return "Length";
        }
    }
}
