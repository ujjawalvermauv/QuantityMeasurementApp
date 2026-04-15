using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    /// <summary>
    /// LengthUnitExtensions - Extension methods for Length unit conversions
    /// 
    /// What it does:
    /// - Provides conversion factors for length units (Feet, Inches, Yards, Centimeters)
    /// - Implements unit-to-base and base-to-unit conversions
    /// - Base unit: FEET (all conversions use feet as reference)
    /// 
    /// Supported Units and Conversion Factors:
    /// - Feet (FEET) = 1.0 (base unit)
    /// - Inches (INCHES) = 1/12 feet (0.0833...)
    /// - Yards (YARDS) = 3 feet
    /// - Centimeters (CENTIMETERS) = 1/30.48 feet (0.03281...)
    /// 
    /// Pattern: Adapter Pattern + IMeasurable Interface
    /// - LengthMeasurable class wraps LengthUnit enum
    /// - Allows LengthUnit to be treated as IMeasurable interface
    /// - Provides polymorphic conversion behavior
    /// </summary>
    public static class LengthUnitExtensions
    {
        /// <summary>
        /// LengthMeasurable - Adapter class implementing IMeasurable for LengthUnit
        /// 
        /// Pattern: Adapter Pattern - wraps enum to implement interface
        /// Purpose: Allows LengthUnit to be used polymorphically as IMeasurable
        /// </summary>
        private sealed class LengthMeasurable : IMeasurable
        {
            private readonly LengthUnit unit;

            public LengthMeasurable(LengthUnit unit)
            {
                this.unit = unit;
            }

            public double GetConversionFactor() => unit.GetConversionFactor();

            public double ConvertToBaseUnit(double value) => unit.ConvertToBaseUnit(value);

            public double ConvertFromBaseUnit(double baseValue) =>
                unit.ConvertFromBaseUnit(baseValue);

            public string GetUnitName() => unit.GetUnitName();
        }

        /// <summary>
        /// Gets conversion factor for length unit relative to base unit (FEET)
        /// 
        /// Parameters:
        /// - unit: Length unit enum value
        /// 
        /// Returns:
        /// - Conversion factor (multiply by this to convert to base unit in feet)
        /// 
        /// Examples:
        /// - LengthUnit.Feet.GetConversionFactor() = 1.0
        /// - LengthUnit.Inches.GetConversionFactor() = 1/12 (0.0833...)
        /// - LengthUnit.Yards.GetConversionFactor() = 3.0
        /// - LengthUnit.Centimeters.GetConversionFactor() = 1/30.48
        /// </summary>
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => 1.0,
                LengthUnit.Inches => 1.0 / 12.0,
                LengthUnit.Yards => 3.0,
                LengthUnit.Centimeters => 1.0 / 30.48,
                _ => throw new ArgumentException($"Unsupported unit: {unit}"),
            };
        }

        /// <summary>
        /// Converts length value to base unit (FEET)
        /// 
        /// Formula: baseValue = value * GetConversionFactor()
        /// 
        /// Examples:
        /// - 12 inches.ConvertToBaseUnit() = 1 foot
        /// - 100 centimeters.ConvertToBaseUnit() ≈ 3.28 feet
        /// </summary>
        public static double ConvertToBaseUnit(this LengthUnit unit, double value) =>
            value * unit.GetConversionFactor();

        /// <summary>
        /// Converts base unit value (FEET) to specified length unit
        /// 
        /// Formula: convertedValue = baseValue / GetConversionFactor()
        /// Inverse operation of ConvertToBaseUnit()
        /// 
        /// Examples:
        /// - 1 foot.ConvertFromBaseUnit(LengthUnit.Inches) = 12 inches
        /// - 1 foot.ConvertFromBaseUnit(LengthUnit.Centimeters) ≈ 30.48 cm
        /// </summary>
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue) =>
            baseValue / unit.GetConversionFactor();

        /// <summary>
        /// Gets readable name for length unit
        /// 
        /// Returns uppercase string representation of enum (e.g., "FEET", "INCHES")
        /// 
        /// Used for:
        /// - Display purposes
        /// - User-facing output
        /// - Logging and reporting
        /// </summary>
        public static string GetUnitName(this LengthUnit unit) =>
            unit.ToString().ToUpperInvariant();

        /// <summary>
        /// Adapts LengthUnit enum to IMeasurable interface
        /// 
        /// Returns:
        /// - LengthMeasurable adapter instance wrapping the unit
        /// 
        /// Purpose:
        /// - Enables polymorphic treatment of units
        /// - Allows units to be used with generic code expecting IMeasurable
        /// 
        /// Usage:
        /// IMeasurable measurable = LengthUnit.Feet.AsMeasurable();
        /// </summary>
        public static IMeasurable AsMeasurable(this LengthUnit unit) => new LengthMeasurable(unit);
    }
}
