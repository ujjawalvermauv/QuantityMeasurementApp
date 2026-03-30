using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    public static class WeightUnitExtensions
    {
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

        public static double ConvertToBaseUnit(this WeightUnit unit, double value) =>
            value * unit.GetConversionFactor();

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue) =>
            baseValue / unit.GetConversionFactor();

        public static string GetUnitName(this WeightUnit unit) =>
            unit.ToString().ToUpperInvariant();

        public static IMeasurable AsMeasurable(this WeightUnit unit) => new WeightMeasurable(unit);
    }
}
