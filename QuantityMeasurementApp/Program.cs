using System;

namespace QuantityMeasurementApp
{
    public interface IMeasurable
    {
    }

    public enum LengthUnit
    {
        FEET,
        INCH,
        YARD,
        CENTIMETER
    }

    public enum WeightUnit
    {
        KILOGRAM,
        GRAM
    }

    public enum VolumeUnit
    {
        LITRE,
        MILLILITRE
    }

    public sealed class Quantity<U> where U : struct, Enum
    {
        private const double EPSILON = 1e-9;

        public Quantity(double value, U unit)
        {
            ValidateFinite(value, nameof(value));
            ValidateEnumUnit(unit, nameof(unit));

            Value = value;
            Unit = unit;
        }

        public double Value { get; }

        public U Unit { get; }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            ValidateEnumUnit(targetUnit, nameof(targetUnit));

            double baseValue = UnitConverter.ToBase(Value, Unit);
            double converted = UnitConverter.FromBase(baseValue, targetUnit);

            return new Quantity<U>(RoundToTwo(converted), targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            return Add(other, Unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            ValidateOperand(other);
            ValidateEnumUnit(targetUnit, nameof(targetUnit));

            double resultInBase = UnitConverter.ToBase(Value, Unit) + UnitConverter.ToBase(other.Value, other.Unit);
            double resultInTarget = UnitConverter.FromBase(resultInBase, targetUnit);

            return new Quantity<U>(RoundToTwo(resultInTarget), targetUnit);
        }

        public Quantity<U> Subtract(Quantity<U> other)
        {
            return Subtract(other, Unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            ValidateOperand(other);
            ValidateEnumUnit(targetUnit, nameof(targetUnit));

            double resultInBase = UnitConverter.ToBase(Value, Unit) - UnitConverter.ToBase(other.Value, other.Unit);
            double resultInTarget = UnitConverter.FromBase(resultInBase, targetUnit);

            return new Quantity<U>(RoundToTwo(resultInTarget), targetUnit);
        }

        public double Divide(Quantity<U> other)
        {
            ValidateOperand(other);

            double dividend = UnitConverter.ToBase(Value, Unit);
            double divisor = UnitConverter.ToBase(other.Value, other.Unit);

            if (Math.Abs(divisor) < EPSILON)
            {
                throw new ArithmeticException("Cannot divide by zero quantity.");
            }

            return dividend / divisor;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not Quantity<U> other)
            {
                return false;
            }

            double thisInBase = UnitConverter.ToBase(Value, Unit);
            double otherInBase = UnitConverter.ToBase(other.Value, other.Unit);

            return Math.Abs(thisInBase - otherInBase) < EPSILON;
        }

        public override int GetHashCode()
        {
            return UnitConverter.ToBase(Value, Unit).GetHashCode();
        }

        public override string ToString()
        {
            return $"Quantity({RoundToTwo(Value)}, {Unit})";
        }

        private static void ValidateFinite(double value, string paramName)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Value must be a finite number.", paramName);
            }
        }

        private static void ValidateEnumUnit(U unit, string paramName)
        {
            if (!Enum.IsDefined(typeof(U), unit))
            {
                throw new ArgumentException("Invalid unit value.", paramName);
            }
        }

        private static void ValidateOperand(Quantity<U>? other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            ValidateFinite(other.Value, nameof(other.Value));
            ValidateEnumUnit(other.Unit, nameof(other.Unit));
        }

        private static double RoundToTwo(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }

    public static class UnitConverter
    {
        private const double INCH_TO_FEET = 1.0 / 12.0;
        private const double YARD_TO_FEET = 3.0;
        private const double CM_TO_FEET = 0.0328084;
        private const double GRAM_TO_KILOGRAM = 0.001;
        private const double MILLILITRE_TO_LITRE = 0.001;

        public static double ToBase<U>(double value, U unit) where U : struct, Enum
        {
            if (typeof(U) == typeof(LengthUnit))
            {
                var lengthUnit = (LengthUnit)(object)unit;
                return lengthUnit switch
                {
                    LengthUnit.FEET => value,
                    LengthUnit.INCH => value * INCH_TO_FEET,
                    LengthUnit.YARD => value * YARD_TO_FEET,
                    LengthUnit.CENTIMETER => value * CM_TO_FEET,
                    _ => throw new ArgumentException("Unsupported length unit.")
                };
            }

            if (typeof(U) == typeof(WeightUnit))
            {
                var weightUnit = (WeightUnit)(object)unit;
                return weightUnit switch
                {
                    WeightUnit.KILOGRAM => value,
                    WeightUnit.GRAM => value * GRAM_TO_KILOGRAM,
                    _ => throw new ArgumentException("Unsupported weight unit.")
                };
            }

            if (typeof(U) == typeof(VolumeUnit))
            {
                var volumeUnit = (VolumeUnit)(object)unit;
                return volumeUnit switch
                {
                    VolumeUnit.LITRE => value,
                    VolumeUnit.MILLILITRE => value * MILLILITRE_TO_LITRE,
                    _ => throw new ArgumentException("Unsupported volume unit.")
                };
            }

            throw new ArgumentException($"Unsupported unit category: {typeof(U).Name}");
        }

        public static double FromBase<U>(double value, U unit) where U : struct, Enum
        {
            if (typeof(U) == typeof(LengthUnit))
            {
                var lengthUnit = (LengthUnit)(object)unit;
                return lengthUnit switch
                {
                    LengthUnit.FEET => value,
                    LengthUnit.INCH => value / INCH_TO_FEET,
                    LengthUnit.YARD => value / YARD_TO_FEET,
                    LengthUnit.CENTIMETER => value / CM_TO_FEET,
                    _ => throw new ArgumentException("Unsupported length unit.")
                };
            }

            if (typeof(U) == typeof(WeightUnit))
            {
                var weightUnit = (WeightUnit)(object)unit;
                return weightUnit switch
                {
                    WeightUnit.KILOGRAM => value,
                    WeightUnit.GRAM => value / GRAM_TO_KILOGRAM,
                    _ => throw new ArgumentException("Unsupported weight unit.")
                };
            }

            if (typeof(U) == typeof(VolumeUnit))
            {
                var volumeUnit = (VolumeUnit)(object)unit;
                return volumeUnit switch
                {
                    VolumeUnit.LITRE => value,
                    VolumeUnit.MILLILITRE => value / MILLILITRE_TO_LITRE,
                    _ => throw new ArgumentException("Unsupported volume unit.")
                };
            }

            throw new ArgumentException($"Unsupported unit category: {typeof(U).Name}");
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            DemonstrateSubtraction();
            Console.WriteLine();
            DemonstrateDivision();
        }

        private static void DemonstrateSubtraction()
        {
            Console.WriteLine("UC12 Subtraction Examples:");

            var lengthImplicit = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH));

            var lengthExplicit = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH), LengthUnit.INCH);

            var weightImplicit = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM)
                .Subtract(new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM));

            var volumeExplicit = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE)
                .Subtract(new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE), VolumeUnit.MILLILITRE);

            Console.WriteLine($"10 FEET - 6 INCHES = {lengthImplicit}");
            Console.WriteLine($"10 FEET - 6 INCHES (in INCH) = {lengthExplicit}");
            Console.WriteLine($"10 KILOGRAM - 5000 GRAM = {weightImplicit}");
            Console.WriteLine($"5 LITRE - 2 LITRE (in MILLILITRE) = {volumeExplicit}");
        }

        private static void DemonstrateDivision()
        {
            Console.WriteLine("UC12 Division Examples:");

            double lengthRatio = new Quantity<LengthUnit>(24.0, LengthUnit.INCH)
                .Divide(new Quantity<LengthUnit>(2.0, LengthUnit.FEET));

            double weightRatio = new Quantity<WeightUnit>(2000.0, WeightUnit.GRAM)
                .Divide(new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM));

            double volumeRatio = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE)
                .Divide(new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE));

            Console.WriteLine($"24 INCH / 2 FEET = {lengthRatio}");
            Console.WriteLine($"2000 GRAM / 1 KILOGRAM = {weightRatio}");
            Console.WriteLine($"5 LITRE / 10 LITRE = {volumeRatio}");
        }
    }
}