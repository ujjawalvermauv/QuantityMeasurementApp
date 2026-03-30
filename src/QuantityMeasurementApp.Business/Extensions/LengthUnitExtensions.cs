using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    public static class LengthUnitExtensions
    {
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

        public static double ConvertToBaseUnit(this LengthUnit unit, double value) =>
            value * unit.GetConversionFactor();

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue) =>
            baseValue / unit.GetConversionFactor();

        public static string GetUnitName(this LengthUnit unit) =>
            unit.ToString().ToUpperInvariant();

        public static IMeasurable AsMeasurable(this LengthUnit unit) => new LengthMeasurable(unit);
    }
}
