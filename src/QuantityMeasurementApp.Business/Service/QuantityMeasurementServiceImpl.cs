using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Business
{
    /// <summary>
    /// Business-layer service that orchestrates quantity operations for both
    /// strongly typed domain models and DTO-based application flows.
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        public QuantityDTO Convert(QuantityDTO source, string targetUnit)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(source);
                return source.Category switch
                {
                    MeasurementCategory.Length => ConvertTypedToDto<LengthUnit>(source, targetUnit),
                    MeasurementCategory.Weight => ConvertTypedToDto<WeightUnit>(source, targetUnit),
                    MeasurementCategory.Volume => ConvertTypedToDto<VolumeUnit>(source, targetUnit),
                    MeasurementCategory.Temperature => ConvertTypedToDto<TemperatureUnit>(
                        source,
                        targetUnit
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(source)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Conversion failed", ex);
            }
        }

        public bool Compare(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(first);
                ArgumentNullException.ThrowIfNull(second);

                EnsureSameCategory(first, second);

                return first.Category switch
                {
                    MeasurementCategory.Length => AreEqualTyped<LengthUnit>(first, second),
                    MeasurementCategory.Weight => AreEqualTyped<WeightUnit>(first, second),
                    MeasurementCategory.Volume => AreEqualTyped<VolumeUnit>(first, second),
                    MeasurementCategory.Temperature => AreEqualTyped<TemperatureUnit>(
                        first,
                        second
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(first)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Comparison failed", ex);
            }
        }

        public QuantityDTO Add(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(a);
                ArgumentNullException.ThrowIfNull(b);

                EnsureSameCategory(a, b);

                return a.Category switch
                {
                    MeasurementCategory.Length => AddTyped<LengthUnit>(a, b, targetUnit),
                    MeasurementCategory.Weight => AddTyped<WeightUnit>(a, b, targetUnit),
                    MeasurementCategory.Volume => AddTyped<VolumeUnit>(a, b, targetUnit),
                    MeasurementCategory.Temperature => throw new InvalidOperationException(
                        "Temperature addition is not supported."
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(a)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Addition failed", ex);
            }
        }

        public QuantityDTO Subtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(a);
                ArgumentNullException.ThrowIfNull(b);

                EnsureSameCategory(a, b);

                return a.Category switch
                {
                    MeasurementCategory.Length => SubtractTyped<LengthUnit>(a, b, targetUnit),
                    MeasurementCategory.Weight => SubtractTyped<WeightUnit>(a, b, targetUnit),
                    MeasurementCategory.Volume => SubtractTyped<VolumeUnit>(a, b, targetUnit),
                    MeasurementCategory.Temperature => throw new InvalidOperationException(
                        "Temperature subtraction is not supported."
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(a)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Subtraction failed", ex);
            }
        }

        public double Divide(QuantityDTO a, QuantityDTO b)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(a);
                ArgumentNullException.ThrowIfNull(b);

                EnsureSameCategory(a, b);

                return a.Category switch
                {
                    MeasurementCategory.Length => DivideTyped<LengthUnit>(a, b),
                    MeasurementCategory.Weight => DivideTyped<WeightUnit>(a, b),
                    MeasurementCategory.Volume => DivideTyped<VolumeUnit>(a, b),
                    MeasurementCategory.Temperature => throw new InvalidOperationException(
                        "Temperature division is not supported."
                    ),
                    _ => throw new ArgumentException(
                        "Unsupported measurement category.",
                        nameof(a)
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exceptions.QuantityMeasurementException("Division failed", ex);
            }
        }

        private static void EnsureSameCategory(QuantityDTO first, QuantityDTO second)
        {
            if (first.Category != second.Category)
                throw new ArgumentException("Both quantities must belong to the same category.");
        }

        private static QuantityDTO ConvertTypedToDto<U>(QuantityDTO source, string targetUnit)
            where U : struct, Enum
        {
            var sourceUnit = ParseUnit<U>(source.Unit);
            var target = ParseUnit<U>(targetUnit);
            var converted = new Quantity<U>(source.Value, sourceUnit).ConvertTo(target);
            return new QuantityDTO(converted.Value, converted.Unit.ToString(), source.Category);
        }

        private static bool AreEqualTyped<U>(QuantityDTO first, QuantityDTO second)
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));
            return left.Equals(right);
        }

        private static QuantityDTO AddTyped<U>(
            QuantityDTO first,
            QuantityDTO second,
            string? targetUnit
        )
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));

            Quantity<U> result = string.IsNullOrWhiteSpace(targetUnit)
                ? left.Add(right)
                : left.Add(right, ParseUnit<U>(targetUnit));

            return new QuantityDTO(result.Value, result.Unit.ToString(), first.Category);
        }

        private static QuantityDTO SubtractTyped<U>(
            QuantityDTO first,
            QuantityDTO second,
            string? targetUnit
        )
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));

            Quantity<U> result = string.IsNullOrWhiteSpace(targetUnit)
                ? left.Subtract(right)
                : left.Subtract(right, ParseUnit<U>(targetUnit));

            return new QuantityDTO(result.Value, result.Unit.ToString(), first.Category);
        }

        private static double DivideTyped<U>(QuantityDTO first, QuantityDTO second)
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));
            return left.Divide(right);
        }

        private static U ParseUnit<U>(string unitName)
            where U : struct, Enum
        {
            if (!Enum.TryParse(unitName, ignoreCase: true, out U parsed))
                throw new ArgumentException($"Unsupported unit '{unitName}' for {typeof(U).Name}.");

            return parsed;
        }
    }
}
