using System;

namespace QuantityMeasurementApp
{
    public enum LengthUnit
    {
        FEET,
        INCH,
        YARD,
        CENTIMETER
    }

    public static class LengthUnitExtensions
    {
        private const double INCH_TO_FEET = 1.0 / 12.0;
        private const double YARD_TO_FEET = 3.0;
        private const double CENTIMETER_TO_FEET = 0.0328084;

        public static double GetConversionFactorToFeet(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET => 1.0,
                LengthUnit.INCH => INCH_TO_FEET,
                LengthUnit.YARD => YARD_TO_FEET,
                LengthUnit.CENTIMETER => CENTIMETER_TO_FEET,
                _ => throw new ArgumentException("Unsupported length unit", nameof(unit))
            };
        }

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value", nameof(value));

            return value * unit.GetConversionFactorToFeet();
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            if (double.IsNaN(baseValue) || double.IsInfinity(baseValue))
                throw new ArgumentException("Invalid numeric value", nameof(baseValue));

            return baseValue / unit.GetConversionFactorToFeet();
        }
    }
}
