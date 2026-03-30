using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    public static class TemperatureUnitExtensions
    {
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

            public bool SupportsArithmetic()
            {
                SupportsArithmetic sa = () => false;
                return sa();
            }

            public void ValidateOperationSupport(string operation)
            {
                throw new NotSupportedException(
                    $"Temperature does not support {operation} operation for absolute values."
                );
            }
        }

        public static double GetConversionFactor(this TemperatureUnit unit) => 1.0;

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

        public static string GetUnitName(this TemperatureUnit unit) =>
            unit.ToString().ToUpperInvariant();

        public static IMeasurable AsMeasurable(this TemperatureUnit unit) =>
            new TemperatureMeasurable(unit);
    }
}
