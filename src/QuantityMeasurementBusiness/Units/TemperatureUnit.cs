namespace QuantityMeasurementApp.QuantityMeasurementBusiness.Units
{
    /// <summary>
    /// Temperature measurement units supporting Celsius, Fahrenheit, and Kelvin.
    /// Temperature units support conversion and equality but not arithmetic operations.
    /// </summary>
    public enum TemperatureUnit
    {
        /// <summary>
        /// Celsius temperature scale (base unit)
        /// </summary>
        CELSIUS,

        /// <summary>
        /// Fahrenheit temperature scale
        /// </summary>
        FAHRENHEIT,

        /// <summary>
        /// Kelvin absolute temperature scale
        /// </summary>
        KELVIN
    }
}