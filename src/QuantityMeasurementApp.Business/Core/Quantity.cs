using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Business
{
    internal interface IQuantityComparable
    {
        Type UnitType { get; }
        double BaseValue { get; }
    }

    internal static class MeasurableResolver
    {
        public static bool TryResolve<U>(U unit, out IMeasurable measurable)
            where U : struct, Enum
        {
            measurable = default!;

            if (!Enum.IsDefined(typeof(U), unit))
                return false;

            if (unit is LengthUnit lengthUnit)
            {
                measurable = lengthUnit.AsMeasurable();
                return true;
            }

            if (unit is WeightUnit weightUnit)
            {
                measurable = weightUnit.AsMeasurable();
                return true;
            }

            if (unit is VolumeUnit volumeUnit)
            {
                measurable = volumeUnit.AsMeasurable();
                return true;
            }

            if (unit is TemperatureUnit temperatureUnit)
            {
                measurable = temperatureUnit.AsMeasurable();
                return true;
            }

            return false;
        }
    }

    public sealed class Quantity<U> : IEquatable<Quantity<U>>, IQuantityComparable
        where U : struct, Enum
    {
        private const double Epsilon = 1e-6;

        private enum ArithmeticOperation
        {
            Add,
            Subtract,
            Divide,
        }

        public double Value { get; }
        public U Unit { get; }

        public Type UnitType => Unit.GetType();

        public Quantity(double value, U unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be a finite number.", nameof(value));

            if (!MeasurableResolver.TryResolve(unit, out _))
                throw new ArgumentException($"Unsupported unit: {unit}", nameof(unit));

            Value = value;
            Unit = unit;
        }

        private IMeasurable ResolveMeasurable(U unit)
        {
            if (!MeasurableResolver.TryResolve(unit, out var measurable))
                throw new ArgumentException($"Unsupported unit: {unit}", nameof(unit));

            return measurable;
        }

        public bool Equals(Quantity<U>? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return Equals((object)other);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is not IQuantityComparable other)
                return false;

            if (Unit.GetType() != other.UnitType)
                return false;

            return Math.Abs(BaseValue - other.BaseValue) < Epsilon;
        }

        public override int GetHashCode()
        {
            long normalized = (long)Math.Round(BaseValue / Epsilon, MidpointRounding.AwayFromZero);
            return HashCode.Combine(Unit.GetType(), normalized);
        }

        public override string ToString() => $"{Value:0.##} {Unit}";

        public double BaseValue => ResolveMeasurable(Unit).ConvertToBaseUnit(Value);

        public Quantity<U> ConvertTo(U targetUnit)
        {
            if (!MeasurableResolver.TryResolve(targetUnit, out var targetMeasurable))
                throw new ArgumentException($"Unsupported unit: {targetUnit}", nameof(targetUnit));

            double valueInBaseUnit = ResolveMeasurable(Unit).ConvertToBaseUnit(Value);
            double convertedValue = targetMeasurable.ConvertFromBaseUnit(valueInBaseUnit);
            return new Quantity<U>(Math.Round(convertedValue, 2), targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other) => Add(other, Unit);

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(
                other,
                targetUnit,
                targetUnitRequired: true,
                operation: ArithmeticOperation.Add
            );
            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.Add);
            return ConvertBaseResultToQuantity(baseResult, targetUnit);
        }

        public Quantity<U> Add(double value, U unit) => Add(new Quantity<U>(value, unit));

        public Quantity<U> Add(double value, U unit, U targetUnit) =>
            Add(new Quantity<U>(value, unit), targetUnit);

        public Quantity<U> Subtract(Quantity<U> other) => Subtract(other, Unit);

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(
                other,
                targetUnit,
                targetUnitRequired: true,
                operation: ArithmeticOperation.Subtract
            );
            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.Subtract);
            return ConvertBaseResultToQuantity(baseResult, targetUnit);
        }

        public double Divide(Quantity<U> other)
        {
            ValidateArithmeticOperands(
                other,
                null,
                targetUnitRequired: false,
                operation: ArithmeticOperation.Divide
            );
            return PerformBaseArithmetic(other, ArithmeticOperation.Divide);
        }

        private void ValidateArithmeticOperands(
            Quantity<U> other,
            U? targetUnit,
            bool targetUnitRequired,
            ArithmeticOperation operation
        )
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (Unit.GetType() != other.Unit.GetType())
                throw new ArgumentException(
                    "Quantities must belong to the same measurement category.",
                    nameof(other)
                );

            ValidateFiniteQuantity(this, nameof(Value));
            ValidateFiniteQuantity(other, nameof(other));

            ResolveMeasurable(Unit).ValidateOperationSupport(operation.ToString());
            other.ResolveMeasurable(other.Unit).ValidateOperationSupport(operation.ToString());

            if (!targetUnitRequired)
                return;

            if (!targetUnit.HasValue)
                throw new ArgumentException("Target unit is required.", nameof(targetUnit));

            if (!MeasurableResolver.TryResolve(targetUnit.Value, out _))
                throw new ArgumentException(
                    $"Unsupported unit: {targetUnit.Value}",
                    nameof(targetUnit)
                );
        }

        private double PerformBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            double thisValueInBase = ResolveMeasurable(Unit).ConvertToBaseUnit(Value);
            double otherValueInBase = other
                .ResolveMeasurable(other.Unit)
                .ConvertToBaseUnit(other.Value);
            return operation switch
            {
                ArithmeticOperation.Add => thisValueInBase + otherValueInBase,
                ArithmeticOperation.Subtract => thisValueInBase - otherValueInBase,
                ArithmeticOperation.Divide => DivideBaseValues(thisValueInBase, otherValueInBase),
                _ => throw new InvalidOperationException($"Unsupported operation: {operation}"),
            };
        }

        private Quantity<U> ConvertBaseResultToQuantity(double baseResult, U targetUnit)
        {
            double convertedValue = ResolveMeasurable(targetUnit).ConvertFromBaseUnit(baseResult);
            return new Quantity<U>(Math.Round(convertedValue, 2), targetUnit);
        }

        private static double DivideBaseValues(double dividend, double divisor)
        {
            if (Math.Abs(divisor) < Epsilon)
                throw new ArithmeticException("Cannot divide by zero quantity.");

            return dividend / divisor;
        }

        private static void ValidateFiniteQuantity(Quantity<U> quantity, string parameterName)
        {
            if (!double.IsFinite(quantity.Value))
                throw new ArgumentException("Quantity value must be finite.", parameterName);

            if (!MeasurableResolver.TryResolve(quantity.Unit, out _))
                throw new ArgumentException($"Unsupported unit: {quantity.Unit}", parameterName);
        }
    }
}
