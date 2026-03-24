namespace QuantityMeasurementApp.QuantityMeasurementBusiness.Units
{
    /// <summary>
    /// Extension methods for TemperatureUnit conversion behavior.
    /// </summary>
    public static class TemperatureUnitExtensions
    {
        /// <summary>
        /// Converts a temperature value to Celsius (base unit).
        /// </summary>
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => value,
                TemperatureUnit.FAHRENHEIT => (value - 32.0) * 5.0 / 9.0,
                TemperatureUnit.KELVIN => value - 273.15,
                _ => throw new ArgumentException("Unsupported temperature unit")
            };
        }

        /// <summary>
        /// Converts a temperature value from Celsius (base unit) to the target unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => baseValue,
                TemperatureUnit.FAHRENHEIT => (baseValue * 9.0 / 5.0) + 32.0,
                TemperatureUnit.KELVIN => baseValue + 273.15,
                _ => throw new ArgumentException("Unsupported temperature unit")
            };
        }

        /// <summary>
        /// Gets the readable name for the temperature unit.
        /// </summary>
        public static string GetUnitName(this TemperatureUnit unit)
        {
            return unit.ToString();
        }

        /// <summary>
        /// Validates that the specified operation is supported by temperature units.
        /// Temperature units do not support arithmetic operations.
        /// </summary>
        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            if (operation == "ADD" || operation == "SUBTRACT" || operation == "DIVIDE")
            {
                throw new NotSupportedException($"Temperature does not support {operation.ToLower()} operation. " +
                    "Temperature arithmetic is not meaningful in most practical contexts.");
            }
        }

        /// <summary>
        /// Returns the measurement type.
        /// </summary>
        public static string GetMeasurementType(this TemperatureUnit unit)
        {
            return "Temperature";
        }
    }
}
