using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    public static class VolumeUnitExtensions
    {
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

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value) =>
            value * unit.GetConversionFactor();

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue) =>
            baseValue / unit.GetConversionFactor();

        public static string GetUnitName(this VolumeUnit unit) =>
            unit.ToString().ToUpperInvariant();

        public static IMeasurable AsMeasurable(this VolumeUnit unit) => new VolumeMeasurable(unit);
    }
}
