using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities
{
    /// <summary>
    /// Generic quantity class supporting multiple measurement categories.
    /// Works with unit enums through their extension methods.
    /// </summary>
    /// <typeparam name="U">Unit enum type with conversion extension methods</typeparam>
    public class Quantity<U> where U : Enum
    {
        /// <summary>
        /// Arithmetic operation types for centralized logic
        /// </summary>
        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

        /// <summary>
        /// Quantity value
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Unit type
        /// </summary>
        public U Unit { get; }

        /// <summary>
        /// Initializes a quantity instance.
        /// </summary>
        /// <param name="value">Quantity value</param>
        /// <param name="unit">Measurement unit</param>
        /// <exception cref="ArgumentException">Thrown when unit is null or value is non-finite</exception>
        public Quantity(double value, U unit)
        {
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid quantity value");

            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Converts quantity value to base unit.
        /// </summary>
        /// <returns>Value expressed in base unit</returns>
        private double ConvertToBase()
        {
            return ConvertToBase(Unit, Value);
        }

        /// <summary>
        /// Helper method performing conversion for any supported unit enum.
        /// Pattern-matches on the concrete type to call the appropriate extension method.
        /// Centralizes the logic so dynamic binding is not needed.
        /// </summary>
        private static double ConvertToBase(U unit, double value)
        {
            // note: the cast via pattern matching ensures compile-time knowledge of the type
            if (unit is LengthUnit l)
                return l.ConvertToBaseUnit(value);
            if (unit is WeightUnit w)
                return w.ConvertToBaseUnit(value);
            if (unit is VolumeUnit v)
                return v.ConvertToBaseUnit(value);
            if (unit is TemperatureUnit t)
                return t.ConvertToBaseUnit(value);

            throw new ArgumentException("Unsupported unit type");
        }

        /// <summary>
        /// Converts quantity to target unit.
        /// </summary>
        /// <param name="targetUnit">Target unit for conversion</param>
        /// <returns>New quantity with converted value in target unit</returns>
        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ConvertToBase(Unit, Value);
            double result = ConvertFromBase(targetUnit, baseValue);

            return new Quantity<U>(Math.Round(result, 2), targetUnit);
        }

        private static double ConvertFromBase(U unit, double baseValue)
        {
            if (unit is LengthUnit l)
                return l.ConvertFromBaseUnit(baseValue);
            if (unit is WeightUnit w)
                return w.ConvertFromBaseUnit(baseValue);
            if (unit is VolumeUnit v)
                return v.ConvertFromBaseUnit(baseValue);
            if (unit is TemperatureUnit t)
                return t.ConvertFromBaseUnit(baseValue);

            throw new ArgumentException("Unsupported unit type");
        }

        /// <summary>
        /// Centralized validation for arithmetic operands.
        /// Validates null values, category compatibility, and numeric finiteness.
        /// </summary>
        /// <param name="other">Other quantity operand</param>
        /// <param name="targetUnit">Target unit (required for add/subtract, null for divide)</param>
        /// <param name="targetUnitRequired">Whether target unit validation is required</param>
        /// <exception cref="ArgumentException">Thrown when validation fails</exception>
        private void ValidateArithmeticOperands(Quantity<U> other, U? targetUnit, bool targetUnitRequired)
        {
            if (other == null)
                throw new ArgumentException("Cannot perform arithmetic with null quantity");

            if (targetUnitRequired && targetUnit == null)
                throw new ArgumentException("Target unit cannot be null");

            // Cross-category protection
            if (Unit.GetType() != other.Unit.GetType())
                throw new ArgumentException("Cannot perform arithmetic on quantities of different measurement categories");

            // Finiteness validation
            if (double.IsNaN(Value) || double.IsInfinity(Value))
                throw new ArgumentException("Invalid quantity value (NaN or infinite)");

            if (double.IsNaN(other.Value) || double.IsInfinity(other.Value))
                throw new ArgumentException("Invalid quantity value (NaN or infinite)");
        }

        /// <summary>
        /// Performs base-unit arithmetic operation after validation.
        /// Centralizes conversion and computation logic for all arithmetic operations.
        /// </summary>
        /// <param name="other">Other quantity operand</param>
        /// <param name="operation">Arithmetic operation to perform</param>
        /// <returns>Result in base units</returns>
        /// <exception cref="ArithmeticException">Thrown for division by zero</exception>
        /// <exception cref="NotSupportedException">Thrown when operation is not supported by the unit type</exception>
        private double PerformBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            // Validate operation support for this unit type
            if (Unit is TemperatureUnit tempUnit)
            {
                tempUnit.ValidateOperationSupport(operation.ToString());
            }

            double base1 = ConvertToBase(Unit, Value);
            double base2 = ConvertToBase(other.Unit, other.Value);

            return operation switch
            {
                ArithmeticOperation.ADD => base1 + base2,
                ArithmeticOperation.SUBTRACT => base1 - base2,
                ArithmeticOperation.DIVIDE => base2 == 0.0
                    ? throw new ArithmeticException("Cannot divide by zero quantity")
                    : base1 / base2,
                _ => throw new ArgumentException("Unsupported arithmetic operation")
            };
        }

        /// <summary>
        /// Adds two quantities in the first operand's unit.
        /// </summary>
        /// <param name="other">Quantity to add</param>
        /// <returns>New quantity with sum in first operand's unit</returns>
        public Quantity<U> Add(Quantity<U> other)
        {
            return Add(other, Unit);
        }

        /// <summary>
        /// Adds two quantities with explicit target unit specification.
        /// </summary>
        /// <param name="other">Quantity to add</param>
        /// <param name="targetUnit">Target unit for result</param>
        /// <returns>New quantity with sum in target unit</returns>
        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(other, targetUnit, true);
            double result = PerformBaseArithmetic(other, ArithmeticOperation.ADD);
            double convertedResult = ConvertFromBase(targetUnit, result);

            return new Quantity<U>(Math.Round(convertedResult, 2), targetUnit);
        }

        /// <summary>
        /// Subtracts another quantity from this quantity, returning result in this quantity's unit.
        /// </summary>
        /// <param name="other">Quantity to subtract</param>
        /// <returns>New quantity with difference in first operand's unit</returns>
        /// <exception cref="ArgumentException">Thrown when other is null or from different category</exception>
        public Quantity<U> Subtract(Quantity<U> other)
        {
            return Subtract(other, Unit);
        }

        /// <summary>
        /// Subtracts another quantity from this quantity with explicit target unit specification.
        /// </summary>
        /// <param name="other">Quantity to subtract</param>
        /// <param name="targetUnit">Target unit for result</param>
        /// <returns>New quantity with difference in target unit</returns>
        /// <exception cref="ArgumentException">Thrown when other is null, targetUnit is null, or from different category</exception>
        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(other, targetUnit, true);
            double result = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);
            double convertedResult = ConvertFromBase(targetUnit, result);

            return new Quantity<U>(Math.Round(convertedResult, 2), targetUnit);
        }

        /// <summary>
        /// Divides this quantity by another quantity, returning a dimensionless scalar ratio.
        /// </summary>
        /// <param name="other">Quantity to divide by</param>
        /// <returns>Scalar ratio (dimensionless double)</returns>
        /// <exception cref="ArgumentException">Thrown when other is null or from different category</exception>
        /// <exception cref="ArithmeticException">Thrown when dividing by zero quantity</exception>
        public double Divide(Quantity<U> other)
        {
            ValidateArithmeticOperands(other, default, false);
            return PerformBaseArithmetic(other, ArithmeticOperation.DIVIDE);
        }

        /// <summary>
        /// Compares two quantities for equality using base unit normalization.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if quantities represent equal measurements, false otherwise</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Quantity<U> other = (Quantity<U>)obj;

            // Cross-category protection: ensure units are from same category
            if (Unit.GetType() != other.Unit.GetType())
                return false;

            return Math.Round(ConvertToBase(), 5) == Math.Round(other.ConvertToBase(), 5);
        }

        /// <summary>
        /// Returns hash code based on base unit value for consistency with Equals().
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return ConvertToBase().GetHashCode();
        }

        /// <summary>
        /// Returns string representation of quantity.
        /// </summary>
        /// <returns>Formatted string showing value and unit</returns>
        public override string ToString()
        {
            var name = GetUnitName(Unit);
            return $"Quantity({Value}, {name})";
        }

        private static string GetUnitName(U unit)
        {
            if (unit is LengthUnit l)
                return l.GetUnitName();
            if (unit is WeightUnit w)
                return w.GetUnitName();
            if (unit is VolumeUnit v)
                return v.GetUnitName();
            if (unit is TemperatureUnit t)
                return t.GetUnitName();

            throw new ArgumentException("Unsupported unit type");
        }
    }
}