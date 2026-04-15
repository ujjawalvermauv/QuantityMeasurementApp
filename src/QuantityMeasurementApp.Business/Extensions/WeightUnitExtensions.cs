using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    /// <summary>
    /// WeightUnitExtensions - Extension methods for Weight unit conversions
    /// 
    /// What it does:
    /// - Provides conversion factors for weight units (Kilogram, Gram, Pound)
    /// - Implements unit-to-base and base-to-unit conversions
    /// - Base unit: KILOGRAM (all conversions use kilograms as reference)
    /// 
    /// Supported Units and Conversion Factors:
    /// - Kilogram (KILOGRAM) = 1.0 (base unit)
    /// - Gram (GRAM) = 0.001 kg
    /// - Pound (POUND) = 0.453592 kg (1 lb ≈ 0.4536 kg)
    /// 
    /// Why Kilogram as base:
    /// - ISO standard unit for mass
    /// - Commonly used in metric system
    /// - Easy to convert other units to kilograms
    /// 
    /// Pattern: Adapter Pattern + IMeasurable Interface
    /// - WeightMeasurable class wraps WeightUnit enum
    /// - Allows WeightUnit to be treated as IMeasurable interface
    /// - Provides polymorphic conversion behavior
    /// </summary>
    public static class WeightUnitExtensions
    {
        /// <summary>
        /// WeightMeasurable - Adapter class implementing IMeasurable for WeightUnit
        /// 
        /// Pattern: Adapter Pattern - wraps enum to implement interface
        /// Purpose: Allows WeightUnit to be used polymorphically as IMeasurable
        /// </summary>
        private sealed class WeightMeasurable : IMeasurable
        {
            private readonly WeightUnit unit;

            public WeightMeasurable(WeightUnit unit)
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
        /// Gets conversion factor for weight unit relative to base unit (KILOGRAM)
        /// 
        /// Parameters:
        /// - unit: Weight unit enum value
        /// 
        /// Returns:
        /// - Conversion factor (multiply by this to convert to base unit in kilograms)
        /// 
        /// Examples:
        /// - WeightUnit.Kilogram.GetConversionFactor() = 1.0
        /// - WeightUnit.Gram.GetConversionFactor() = 0.001
        /// - WeightUnit.Pound.GetConversionFactor() ≈ 0.453592
        /// </summary>
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Kilogram => 1.0,
                WeightUnit.Gram => 0.001,
                WeightUnit.Pound => 0.453592,
                _ => throw new ArgumentException($"Unsupported unit: {unit}"),
            };
        }

        /// <summary>
        /// Converts weight value to base unit (KILOGRAM)
        /// 
        /// Formula: baseValue = value * GetConversionFactor()
        /// 
        /// Examples:
        /// - 1000 grams.ConvertToBaseUnit() = 1 kilogram
        /// - 2.20462 pounds.ConvertToBaseUnit() ≈ 1 kilogram
        /// </summary>
        public static double ConvertToBaseUnit(this WeightUnit unit, double value) =>
            value * unit.GetConversionFactor();

        /// <summary>
        /// Converts base unit value (KILOGRAM) to specified weight unit
        /// 
        /// Formula: convertedValue = baseValue / GetConversionFactor()
        /// Inverse operation of ConvertToBaseUnit()
        /// 
        /// Examples:
        /// - 1 kg.ConvertFromBaseUnit(WeightUnit.Gram) = 1000 grams
        /// - 1 kg.ConvertFromBaseUnit(WeightUnit.Pound) ≈ 2.20462 pounds
        /// </summary>
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue) =>
            baseValue / unit.GetConversionFactor();

        /// <summary>
        /// Gets readable name for weight unit
        /// 
        /// Returns uppercase string representation of enum (e.g., "KILOGRAM", "GRAM", "POUND")
        /// 
        /// Used for:
        /// - Display purposes
        /// - User-facing output
        /// - Logging and reporting
        /// </summary>
        public static string GetUnitName(this WeightUnit unit) =>
            unit.ToString().ToUpperInvariant();

        /// <summary>
        /// Adapts WeightUnit enum to IMeasurable interface
        /// 
        /// Returns:
        /// - WeightMeasurable adapter instance wrapping the unit
        /// 
        /// Purpose:
        /// - Enables polymorphic treatment of units
        /// - Allows units to be used with generic code expecting IMeasurable
        /// 
        /// Usage:
        /// IMeasurable measurable = WeightUnit.Kilogram.AsMeasurable();
        /// </summary>
        public static IMeasurable AsMeasurable(this WeightUnit unit) => new WeightMeasurable(unit);
    }
}
