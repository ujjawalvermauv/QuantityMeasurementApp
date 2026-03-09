using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Generic Quantity class that works with any measurement unit implementing IMeasurable.
    /// Replaces category-specific Quantity classes (QuantityLength, QuantityWeight, etc.)
    /// Provides type-safe operations for equality, conversion, and addition.
    /// </summary>
    public class Quantity<U> where U : struct, Enum
    {
        private readonly double value;
        private readonly U unit;

        private const double EPSILON = 1e-4;

        public Quantity(double value, U unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;

        public U Unit => unit;

        /// <summary>
        /// Converts this quantity to a target unit.
        /// Returns a new Quantity instance (immutability).
        /// </summary>
        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase<U>(value, unit);
            double targetValue = ConvertFromBase<U>(baseValue, targetUnit);
            return new Quantity<U>(targetValue, targetUnit);
        }

        /// <summary>
        /// Adds another quantity to this quantity, returning result in this quantity's unit.
        /// </summary>
        public Quantity<U> Add(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Add(this, other, this.unit);
        }

        /// <summary>
        /// Static method: adds two quantities, returning result in first operand's unit.
        /// </summary>
        public static Quantity<U> Add(Quantity<U> first, Quantity<U> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));

            return Add(first, second, first.unit);
        }

        /// <summary>
        /// Static method: adds two quantities with explicit target unit.
        /// </summary>
        public static Quantity<U> Add(Quantity<U> first, Quantity<U> second, U targetUnit)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));

            if (second == null)
                throw new ArgumentNullException(nameof(second));

            double firstInBase = ConvertToBase<U>(first.value, first.unit);
            double secondInBase = ConvertToBase<U>(second.value, second.unit);
            double sumInBase = firstInBase + secondInBase;
            double sumInTarget = ConvertFromBase<U>(sumInBase, targetUnit);

            return new Quantity<U>(sumInTarget, targetUnit);
        }

        /// <summary>
        /// Static method: converts between units of the same category.
        /// </summary>
        public static double Convert(double value, U source, U target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double baseValue = ConvertToBase<U>(value, source);
            return ConvertFromBase<U>(baseValue, target);
        }

        /// <summary>
        /// Helper: converts value in given unit to base unit.
        /// Uses reflection to call GetConversionFactor extension method.
        /// </summary>
        private static double ConvertToBase<T>(double value, T unit) where T : struct, Enum
        {
            var method = typeof(T).Name switch
            {
                "LengthUnit" => typeof(LengthUnitExtensions).GetMethod("ConvertToBaseUnit"),
                "WeightUnit" => typeof(WeightUnitExtensions).GetMethod("ConvertToBaseUnit"),
                "VolumeUnit" => typeof(VolumeUnitExtensions).GetMethod("ConvertToBaseUnit"),
                _ => throw new NotSupportedException($"Unit type {typeof(T).Name} is not supported")
            };

            if (method == null)
                throw new InvalidOperationException($"ConvertToBaseUnit method not found for {typeof(T).Name}");

            return (double)method.Invoke(null, new object[] { unit, value })!;
        }

        /// <summary>
        /// Helper: converts value from base unit to target unit.
        /// Uses reflection to call ConvertFromBaseUnit extension method.
        /// </summary>
        private static double ConvertFromBase<T>(double baseValue, T targetUnit) where T : struct, Enum
        {
            var method = typeof(T).Name switch
            {
                "LengthUnit" => typeof(LengthUnitExtensions).GetMethod("ConvertFromBaseUnit"),
                "WeightUnit" => typeof(WeightUnitExtensions).GetMethod("ConvertFromBaseUnit"),
                "VolumeUnit" => typeof(VolumeUnitExtensions).GetMethod("ConvertFromBaseUnit"),
                _ => throw new NotSupportedException($"Unit type {typeof(T).Name} is not supported")
            };

            if (method == null)
                throw new InvalidOperationException($"ConvertFromBaseUnit method not found for {typeof(T).Name}");

            return (double)method.Invoke(null, new object[] { targetUnit, baseValue })!;
        }

        public override bool Equals(object? obj)
        {
            if (this == obj)
                return true;

            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Quantity<U> other = (Quantity<U>)obj;

            // Prevent cross-category comparison
            if (unit.GetType() != other.unit.GetType())
                return false;

            double thisInBase = ConvertToBase<U>(this.value, this.unit);
            double otherInBase = ConvertToBase<U>(other.value, other.unit);

            return Math.Abs(thisInBase - otherInBase) < EPSILON;
        }

        public override int GetHashCode()
        {
            double baseValue = ConvertToBase<U>(value, unit);
            return baseValue.GetHashCode();
        }

        public override string ToString()
        {
            return $"Quantity({value}, {unit})";
        }
    }
}
