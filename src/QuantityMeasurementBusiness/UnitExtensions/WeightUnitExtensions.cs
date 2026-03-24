namespace QuantityMeasurementApp.QuantityMeasurementBusiness.Units
{
    /// <summary>
    /// Extension methods for WeightUnit conversion behavior.
    /// Provides consistent conversion behavior across all weight units.
    /// </summary>
    public static class WeightUnitExtensions
    {
        /// <summary>
        /// Conversion factors relative to base unit (GRAM).
        /// </summary>
        private static readonly Dictionary<WeightUnit, double> ConversionFactors = new()
        {
            { WeightUnit.KILOGRAM, 1000.0 },
            { WeightUnit.GRAM, 1.0 },
            { WeightUnit.TONNE, 1000000.0 }
        };

        /// <summary>
        /// Gets conversion factor for the unit relative to base unit (GRAM).
        /// </summary>
        public static double GetConversionFactor(this WeightUnit unit)
        {
            if (ConversionFactors.TryGetValue(unit, out var factor))
                return factor;
            throw new ArgumentException("Invalid unit");
        }

        /// <summary>
        /// Converts a value from this unit to the base unit (GRAM).
        /// </summary>
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Converts a value from the base unit (GRAM) to this unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        /// <summary>
        /// Returns the readable unit name.
        /// </summary>
        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString();
        }

        /// <summary>
        /// Returns the measurement type.
        /// </summary>
        public static string GetMeasurementType(this WeightUnit unit)
        {
            return "Weight";
        }
    }
}
