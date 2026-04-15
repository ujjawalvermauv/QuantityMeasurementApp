using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    /// <summary>
    /// VolumeUnitExtensions - Extension methods for Volume unit conversions
    /// 
    /// What it does:
    /// - Provides conversion factors for volume units (Litre, Millilitre, Gallon)
    /// - Implements unit-to-base and base-to-unit conversions
    /// - Base unit: LITRE (all conversions use litres as reference)
    /// 
    /// Supported Units and Conversion Factors:
    /// - Litre (LITRE) = 1.0 (base unit)
    /// - Millilitre (MILLILITRE) = 0.001 litre
    /// - Gallon (GALLON) = 3.78541 litres (US gallon)
    /// 
    /// Why Litre as base:
    /// - ISO standard unit for volume
    /// - Commonly used in metric system
    /// - Easy to convert other units to litres
    /// 
    /// Pattern: Adapter Pattern + IMeasurable Interface
    /// - VolumeMeasurable class wraps VolumeUnit enum
    /// - Allows VolumeUnit to be treated as IMeasurable interface
    /// - Provides polymorphic conversion behavior
    /// </summary>
    public static class VolumeUnitExtensions
    {
        /// <summary>
        /// VolumeMeasurable - Adapter class implementing IMeasurable for VolumeUnit
        /// 
        /// Pattern: Adapter Pattern - wraps enum to implement interface
        /// Purpose: Allows VolumeUnit to be used polymorphically as IMeasurable
        /// </summary>
        private sealed class VolumeMeasurable : IMeasurable
        {
            private readonly VolumeUnit unit;

            public VolumeMeasurable(VolumeUnit unit)
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
        /// Gets conversion factor for volume unit relative to base unit (LITRE)
        /// 
        /// Parameters:
        /// - unit: Volume unit enum value
        /// 
        /// Returns:
        /// - Conversion factor (multiply by this to convert to base unit in litres)
        /// 
        /// Examples:
        /// - VolumeUnit.Litre.GetConversionFactor() = 1.0
        /// - VolumeUnit.Millilitre.GetConversionFactor() = 0.001
        /// - VolumeUnit.Gallon.GetConversionFactor() ≈ 3.78541
        /// </summary>
        public static double GetConversionFactor(this VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.Litre => 1.0,
                VolumeUnit.Millilitre => 0.001,
                VolumeUnit.Gallon => 3.78541,
                _ => throw new ArgumentException($"Unsupported unit: {unit}"),
            };
        }

        /// <summary>
        /// Converts volume value to base unit (LITRE)
        /// 
        /// Formula: baseValue = value * GetConversionFactor()
        /// 
        /// Examples:
        /// - 1000 millilitres.ConvertToBaseUnit() = 1 litre
        /// - 1 gallon.ConvertToBaseUnit() ≈ 3.78541 litres
        /// </summary>
        public static double ConvertToBaseUnit(this VolumeUnit unit, double value) =>
            value * unit.GetConversionFactor();

        /// <summary>
        /// Converts base unit value (LITRE) to specified volume unit
        /// 
        /// Formula: convertedValue = baseValue / GetConversionFactor()
        /// Inverse operation of ConvertToBaseUnit()
        /// 
        /// Examples:
        /// - 1 litre.ConvertFromBaseUnit(VolumeUnit.Millilitre) = 1000 millilitres
        /// - 1 litre.ConvertFromBaseUnit(VolumeUnit.Gallon) ≈ 0.264172 gallons
        /// </summary>
        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue) =>
            baseValue / unit.GetConversionFactor();

        /// <summary>
        /// Gets readable name for volume unit
        /// 
        /// Returns uppercase string representation of enum (e.g., "LITRE", "MILLILITRE", "GALLON")
        /// 
        /// Used for:
        /// - Display purposes
        /// - User-facing output
        /// - Logging and reporting
        /// </summary>
        public static string GetUnitName(this VolumeUnit unit) =>
            unit.ToString().ToUpperInvariant();

        /// <summary>
        /// Adapts VolumeUnit enum to IMeasurable interface
        /// 
        /// Returns:
        /// - VolumeMeasurable adapter instance wrapping the unit
        /// 
        /// Purpose:
        /// - Enables polymorphic treatment of units
        /// - Allows units to be used with generic code expecting IMeasurable
        /// 
        /// Usage:
        /// IMeasurable measurable = VolumeUnit.Litre.AsMeasurable();
        /// </summary>
        public static IMeasurable AsMeasurable(this VolumeUnit unit) => new VolumeMeasurable(unit);
    }
}
