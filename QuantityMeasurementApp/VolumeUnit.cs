using System;

namespace QuantityMeasurementApp
{
    public enum VolumeUnit
    {
        LITRE,
        MILLILITRE,
        GALLON
    }

    public static class VolumeUnitExtensions
    {
        private const double MILLILITRE_TO_LITRE = 0.001;
        private const double GALLON_TO_LITRE = 3.78541;

        public static double GetConversionFactor(this VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.LITRE => 1.0,
                VolumeUnit.MILLILITRE => MILLILITRE_TO_LITRE,
                VolumeUnit.GALLON => GALLON_TO_LITRE,
                _ => throw new ArgumentException("Unsupported volume unit", nameof(unit))
            };
        }

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value", nameof(value));

            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            if (double.IsNaN(baseValue) || double.IsInfinity(baseValue))
                throw new ArgumentException("Invalid numeric value", nameof(baseValue));

            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this VolumeUnit unit)
        {
            return unit.ToString();
        }
    }
}