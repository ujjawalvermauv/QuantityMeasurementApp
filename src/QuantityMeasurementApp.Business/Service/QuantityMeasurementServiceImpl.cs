using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Business
{
    /// <summary>
    /// QuantityMeasurementServiceImpl - Business Logic Service Layer
    /// 
    /// What it does:
    /// - Handles all quantity operations (convert, compare, add, subtract, divide)
    /// - Converts DTOs to strongly typed domain models for type safety
    /// - Provides comprehensive error handling for all operations
    /// 
    /// How it works:
    /// 1. Accepts input as DTO (Data Transfer Object)
    /// 2. Selects appropriate type based on quantity measurement category
    /// 3. Creates strongly typed object (e.g., Quantity<LengthUnit>)
    /// 4. Performs business logic operations
    /// 5. Converts result back to DTO format and returns
    /// 
    /// Why designed this way:
    /// - Type safety: Compile-time checking prevents unit mismatches
    /// - Separation of concerns: Business logic isolated from presentation
    /// - Flexibility: Supports multiple measurement types with single service
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        /// <summary>
        /// Converts a quantity from one unit to another unit
        /// 
        /// Parameters:
        /// - source: Quantity to convert (in DTO format)
        /// - targetUnit: Target unit name (as string, e.g., "METER", "CENTIMETER")
        /// 
        /// Returns:
        /// - Converted quantity in DTO format
        /// 
        /// Example:
        /// Input: QuantityDTO(100, "CENTIMETER", Length)
        /// Convert to: "METER"
        /// Output: QuantityDTO(1, "METER", Length)
        /// 
        /// How it works:
        /// 1. Identifies category (Length, Weight, Temperature, Volume)
        /// 2. Dispatches to appropriate strongly-typed conversion method
        /// 3. Handles exceptions and converts back to DTO
        /// </summary>
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

        /// <summary>
        /// Compares two quantities for equality
        /// 
        /// Returns:
        /// - true if both quantities have equivalent values in base units
        /// - false if they are different
        /// 
        /// Important:
        /// - Units can be different, but values must be equivalent
        /// - Example: 100cm == 1m (returns true)
        /// - Uses epsilon comparison for floating-point precision
        /// 
        /// How it works:
        /// 1. Validates both quantities are not null
        /// 2. Ensures both quantities belong to same category
        /// 3. Converts both to strongly typed objects
        /// 4. Uses type-safe equality comparison
        /// </summary>
        public bool Compare(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(first);
                ArgumentNullException.ThrowIfNull(second);

                // Validate both quantities belong to the same measurement category
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

        /// <summary>
        /// Adds two quantities together
        /// 
        /// Parameters:
        /// - a: First quantity to add
        /// - b: Second quantity to add
        /// - targetUnit: Unit for result (optional, defaults to first quantity's unit)
        /// 
        /// Returns:
        /// - Sum of quantities in specified target unit
        /// 
        /// Example:
        /// a = 1 METER
        /// b = 50 CENTIMETER
        /// targetUnit = "METER"
        /// Returns: 1.5 METER
        /// 
        /// Restrictions:
        /// - NOT supported for Temperature (40°C + 10°C is not meaningful)
        /// - Supported for Length, Weight, Volume
        /// 
        /// Why Temperature addition not supported:
        /// - Absolute temperature values don't add meaningfully
        /// - However, temperature differences (subtraction) are valid
        /// </summary>
        public QuantityDTO Add(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(a);
                ArgumentNullException.ThrowIfNull(b);

                // Validate both quantities belong to same category
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

        /// <summary>
        /// Subtracts one quantity from another
        /// 
        /// Result = a - b (in specified target unit)
        /// 
        /// Supports all measurement types including Temperature
        /// - Temperature subtraction is valid (e.g., 40°C - 30°C = 10°C difference)
        /// </summary>
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

        /// <summary>
        /// Divides one quantity by another
        /// 
        /// Returns:
        /// - Dimensionless ratio (double number without unit)
        /// 
        /// Examples:
        /// - 100 CENTIMETER / 50 CENTIMETER = 2
        /// - 1 METER / 1 METER = 1
        /// 
        /// Why dimensionless:
        /// - Division cancels out units mathematically
        /// - Result is pure ratio with no unit attached
        /// </summary>
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

        /// <summary>
        /// Validates that both quantities belong to the same measurement category
        /// 
        /// Throws ArgumentException if categories don't match
        /// 
        /// Why important:
        /// - Cannot mix units from different categories (1 METER + 1 KG is invalid)
        /// - This is first-line validation before operations
        /// </summary>
        private static void EnsureSameCategory(QuantityDTO first, QuantityDTO second)
        {
            if (first.Category != second.Category)
                throw new ArgumentException("Both quantities must belong to the same category.");
        }

        /// <summary>
        /// Generic helper method - converts DTO to strongly typed Quantity and performs conversion
        /// 
        /// Type Parameter:
        /// - U: Unit enum type (LengthUnit, WeightUnit, VolumeUnit, or TemperatureUnit)
        /// 
        /// How it works:
        /// 1. Parses string unit names to enum values
        /// 2. Creates strongly typed Quantity object
        /// 3. Performs type-safe conversion
        /// 4. Converts result back to DTO
        /// 
        /// Why this pattern:
        /// - Provides compile-time type safety
        /// - Single implementation works for all unit types
        /// - Errors caught at compile time, not runtime
        /// </summary>
        private static QuantityDTO ConvertTypedToDto<U>(QuantityDTO source, string targetUnit)
            where U : struct, Enum
        {
            var sourceUnit = ParseUnit<U>(source.Unit);
            var target = ParseUnit<U>(targetUnit);
            var converted = new Quantity<U>(source.Value, sourceUnit).ConvertTo(target);
            return new QuantityDTO(converted.Value, converted.Unit.ToString(), source.Category);
        }

        /// <summary>
        /// Type-safe equality comparison for two quantities
        /// 
        /// Example: 100cm == 1m returns true
        /// </summary>
        private static bool AreEqualTyped<U>(QuantityDTO first, QuantityDTO second)
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));
            return left.Equals(right);
        }

        /// <summary>
        /// Type-safe addition of two quantities
        /// </summary>
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

        /// <summary>
        /// Type-safe subtraction of two quantities
        /// </summary>
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

        /// <summary>
        /// Type-safe division of two quantities
        /// </summary>
        private static double DivideTyped<U>(QuantityDTO first, QuantityDTO second)
            where U : struct, Enum
        {
            var left = new Quantity<U>(first.Value, ParseUnit<U>(first.Unit));
            var right = new Quantity<U>(second.Value, ParseUnit<U>(second.Unit));
            return left.Divide(right);
        }

        /// <summary>
        /// Parses string unit name to enum value
        /// 
        /// Parameters:
        /// - unitName: String representation (e.g., "METER", "meter", "Meter")
        /// 
        /// Returns:
        /// - Parsed enum value
        /// 
        /// Features:
        /// - Case-insensitive parsing (accepts "METER", "Meter", "meter")
        /// - Throws ArgumentException for invalid units
        /// 
        /// Examples:
        /// - ParseUnit<LengthUnit>("METER") returns LengthUnit.METER
        /// - ParseUnit<LengthUnit>("meter") returns LengthUnit.METER
        /// - ParseUnit<LengthUnit>("INVALID") throws ArgumentException
        /// 
        /// Why case-insensitive:
        /// - User input is often inconsistent
        /// - Improves user experience and flexibility
        /// </summary>
        private static U ParseUnit<U>(string unitName)
            where U : struct, Enum
        {
            if (!Enum.TryParse(unitName, ignoreCase: true, out U parsed))
                throw new ArgumentException($"Unsupported unit '{unitName}' for {typeof(U).Name}.");

            return parsed;
        }
    }
}
