using System;

namespace QuantityMeasurementApp
{
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }

    public static class WeightUnitExtensions
    {
        private const double GRAM_TO_KILOGRAM = 0.001;
        private const double POUND_TO_KILOGRAM = 0.453592;

        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => 1.0,
                WeightUnit.GRAM => GRAM_TO_KILOGRAM,
                WeightUnit.POUND => POUND_TO_KILOGRAM,
                _ => throw new ArgumentException("Unsupported weight unit", nameof(unit))
            };
        }

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value", nameof(value));

            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            if (double.IsNaN(baseValue) || double.IsInfinity(baseValue))
                throw new ArgumentException("Invalid numeric value", nameof(baseValue));

            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString();
        }
    }
}

