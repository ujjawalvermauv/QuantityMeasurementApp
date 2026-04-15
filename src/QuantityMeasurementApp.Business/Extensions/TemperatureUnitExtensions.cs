using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    /// <summary>
    /// TemperatureUnitExtensions - Extension methods for Temperature unit conversions
    /// 
    /// What it does:
    /// - Provides conversion support for temperature units (Celsius, Fahrenheit, Kelvin)
    /// - Implements special conversion logic (non-linear due to different scales and offsets)
    /// - Base unit: CELSIUS (internal reference for conversions)
    /// - Explicitly rejects arithmetic operations (addition/subtraction) on absolute temperatures
    /// 
    /// Supported Units:
    /// - Celsius (CELSIUS) = base unit (0°C = freezing point of water)
    /// - Fahrenheit (FAHRENHEIT) = offset scale (32°F = freezing point)
    /// - Kelvin (KELVIN) = absolute scale (0K = absolute zero, -273.15°C)
    /// 
    /// Critical Design Decision: No Arithmetic on Absolute Temperatures
    /// ================================================================
    /// Temperature is fundamentally different from Length, Weight, Volume:
    /// 
    /// Why you CAN'T add absolute temperatures:
    /// - 40°C + 10°C does NOT equal 50°C
    /// - This is not meaningful in physics or real-world applications
    /// - Adding absolute temperatures is a category error
    /// 
    /// What IS meaningful:
    /// - Temperature differences (deltas): 40°C - 30°C = 10°C difference
    /// - Not currently supported, but could be added as separate operations
    /// 
    /// Conversion Formula Details:
    /// - Celsius ↔ Fahrenheit: Non-linear with offset
    /// - Celsius ↔ Kelvin: Linear shift (K = C + 273.15)
    /// 
    /// Why Celsius as base internally:
    /// - Middle ground between Fahrenheit and Kelvin
    /// - Standard SI unit for temperature differences
    /// 
    /// Pattern: Adapter Pattern + Special IMeasurable Implementation
    /// - TemperatureMeasurable extends IMeasurable
    /// - Overrides SupportsArithmetic() to return false
    /// - Overrides ValidateOperationSupport() to reject operations
    /// </summary>
    public static class TemperatureUnitExtensions
    {
        /// <summary>
        /// TemperatureMeasurable - Special adapter implementing IMeasurable for TemperatureUnit
        /// 
        /// Key Difference from Other Units:
        /// - SupportsArithmetic() returns FALSE (rejects addition/subtraction)
        /// - ValidateOperationSupport() throws NotSupportedException
        /// 
        /// This enforces domain rule: Absolute temperatures cannot be added
        /// </summary>
        private sealed class TemperatureMeasurable : IMeasurable
        {
            private readonly TemperatureUnit unit;

            public TemperatureMeasurable(TemperatureUnit unit)
            {
                this.unit = unit;
            }

            public double GetConversionFactor() => unit.GetConversionFactor();

            public double ConvertToBaseUnit(double value) => unit.ConvertToBaseUnit(value);

            public double ConvertFromBaseUnit(double baseValue) =>
                unit.ConvertFromBaseUnit(baseValue);

            public string GetUnitName() => unit.GetUnitName();

            /// <summary>
            /// Reports that Temperature does NOT support arithmetic operations
            /// 
            /// Returns false to enforce: No addition or subtraction of absolute temperatures
            /// </summary>
            public bool SupportsArithmetic()
            {
                SupportsArithmetic sa = () => false;
                return sa();
            }

            /// <summary>
            /// Validates operation support - REJECTS all arithmetic operations
            /// 
            /// Throws NotSupportedException for Add, Subtract operations
            /// This prevents 40°C + 10°C = meaningless calculations
            /// </summary>
            public void ValidateOperationSupport(string operation)
            {
                throw new NotSupportedException(
                    $"Temperature does not support {operation} operation for absolute values."
                );
            }
        }

        /// <summary>
        /// Gets conversion factor for temperature (always 1.0 as placeholder)
        /// 
        /// Note: Temperature conversion is not based on simple factors
        /// Because temperature scales have different zero points (offsets):
        /// - Celsius: 0°C = water freezes
        /// - Fahrenheit: 32°F = water freezes  
        /// - Kelvin: 0K = absolute zero
        /// 
        /// Due to these offsets, conversion requires formulas, not just multiplication
        /// Returns 1.0 as required by interface, but actual logic is in ConvertToBaseUnit/ConvertFromBaseUnit
        /// </summary>
        public static double GetConversionFactor(this TemperatureUnit unit) => 1.0;

        /// <summary>
        /// Converts temperature value to base unit (CELSIUS)
        /// 
        /// Conversion Formulas:
        /// - Celsius → Celsius: value (no change, already base)
        /// - Fahrenheit → Celsius: (F - 32) × 5/9
        /// - Kelvin → Celsius: K - 273.15
        /// 
        /// Examples:
        /// - 32°F.ConvertToBaseUnit() = 0°C
        /// - 212°F.ConvertToBaseUnit() = 100°C
        /// - 273.15K.ConvertToBaseUnit() = 0°C
        /// - 0K.ConvertToBaseUnit() = -273.15°C
        /// </summary>
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => value,
                TemperatureUnit.Fahrenheit => (value - 32.0) * 5.0 / 9.0,
                TemperatureUnit.Kelvin => value - 273.15,
                _ => throw new ArgumentException($"Unsupported unit: {unit}"),
            };
        }

        /// <summary>
        /// Converts base unit value (CELSIUS) to specified temperature unit
        /// 
        /// Inverse operation of ConvertToBaseUnit()
        /// 
        /// Conversion Formulas:
        /// - Celsius → Celsius: value (no change)
        /// - Celsius → Fahrenheit: (C × 9/5) + 32
        /// - Celsius → Kelvin: C + 273.15
        /// 
        /// Examples:
        /// - 0°C.ConvertFromBaseUnit(Fahrenheit) = 32°F
        /// - 100°C.ConvertFromBaseUnit(Fahrenheit) = 212°F
        /// - 0°C.ConvertFromBaseUnit(Kelvin) = 273.15K
        /// - -273.15°C.ConvertFromBaseUnit(Kelvin) = 0K
        /// </summary>
        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => baseValue,
                TemperatureUnit.Fahrenheit => (baseValue * 9.0 / 5.0) + 32.0,
                TemperatureUnit.Kelvin => baseValue + 273.15,
                _ => throw new ArgumentException($"Unsupported unit: {unit}"),
            };
        }

        /// <summary>
        /// Gets readable name for temperature unit
        /// 
        /// Returns uppercase string representation of enum (e.g., "CELSIUS", "FAHRENHEIT", "KELVIN")
        /// 
        /// Used for:
        /// - Display purposes
        /// - User-facing output
        /// - Logging and reporting
        /// </summary>
        public static string GetUnitName(this TemperatureUnit unit) =>
            unit.ToString().ToUpperInvariant();

        /// <summary>
        /// Adapts TemperatureUnit enum to IMeasurable interface
        /// 
        /// Returns:
        /// - TemperatureMeasurable adapter instance (with arithmetic restrictions)
        /// 
        /// Special Behavior:
        /// - SupportsArithmetic() returns false
        /// - ValidateOperationSupport() rejects all operations
        /// - This enforces domain rule: Temperature addition is not supported
        /// </summary>
        public static IMeasurable AsMeasurable(this TemperatureUnit unit) =>
            new TemperatureMeasurable(unit);
    }
}
