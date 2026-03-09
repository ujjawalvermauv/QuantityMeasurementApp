using System;

namespace QuantityMeasurementApp
{
    // enum for supported length units
    public enum LengthUnit
    {
        FEET,
        INCH,
        YARD,
        CENTIMETER
    }

    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        private const double INCH_TO_FEET = 1.0 / 12.0;
        private const double YARD_TO_FEET = 3.0;
        private const double CM_TO_FEET = 0.0328084; // 1 cm = 0.0328084 feet
        private const double EPSILON = 1e-6;

        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            if (!Enum.IsDefined(typeof(LengthUnit), unit))
                throw new ArgumentException("Invalid length unit");

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;

        public LengthUnit Unit => unit;

        // Convert everything to FEET (base unit)
        private double ToFeet()
        {
            return unit switch
            {
                LengthUnit.FEET => value,
                LengthUnit.INCH => value * INCH_TO_FEET,
                LengthUnit.YARD => value * YARD_TO_FEET,
                LengthUnit.CENTIMETER => value * CM_TO_FEET,
                _ => throw new ArgumentException("Unsupported Unit")
            };
        }

        // -------- UC5 STATIC CONVERSION METHOD --------
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            // Step 1: Convert source → feet
            double valueInFeet = source switch
            {
                LengthUnit.FEET => value,
                LengthUnit.INCH => value * INCH_TO_FEET,
                LengthUnit.YARD => value * YARD_TO_FEET,
                LengthUnit.CENTIMETER => value * CM_TO_FEET,
                _ => throw new ArgumentException("Unsupported Source Unit")
            };

            // Step 2: Convert feet → target
            return target switch
            {
                LengthUnit.FEET => valueInFeet,
                LengthUnit.INCH => valueInFeet / INCH_TO_FEET,
                LengthUnit.YARD => valueInFeet / YARD_TO_FEET,
                LengthUnit.CENTIMETER => valueInFeet / CM_TO_FEET,
                _ => throw new ArgumentException("Unsupported Target Unit")
            };
        }

        // Instance conversion (immutability)
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double convertedValue = Convert(this.value, this.unit, targetUnit);
            return new QuantityLength(convertedValue, targetUnit);
        }

        // -------- UC6 ADDITION METHODS --------
        public QuantityLength Add(QuantityLength other)
        {
            return Add(this, other, this.unit);
        }

        public static QuantityLength Add(QuantityLength first, QuantityLength second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));

            return Add(first, second, first.unit);
        }

        public static QuantityLength Add(QuantityLength first, QuantityLength second, LengthUnit targetUnit)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));

            if (second == null)
                throw new ArgumentNullException(nameof(second));

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid target unit");

            double firstInFeet = first.ToFeet();
            double secondInFeet = second.ToFeet();
            double sumInFeet = firstInFeet + secondInFeet;

            double sumInTarget = Convert(sumInFeet, LengthUnit.FEET, targetUnit);
            return new QuantityLength(sumInTarget, targetUnit);
        }

        public static QuantityLength Add(double firstValue, LengthUnit firstUnit, double secondValue, LengthUnit secondUnit, LengthUnit targetUnit)
        {
            var first = new QuantityLength(firstValue, firstUnit);
            var second = new QuantityLength(secondValue, secondUnit);
            return Add(first, second, targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (this == obj)
                return true;

            if (obj == null || this.GetType() != obj.GetType())
                return false;

            QuantityLength other = (QuantityLength)obj;

            return Math.Abs(this.ToFeet() - other.ToFeet()) < EPSILON;
        }

        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }

        public override string ToString()
        {
            return $"Quantity({value}, {unit})";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // UC4 Equality Check
            var q1 = new QuantityLength(1.0, LengthUnit.YARD);
            var q2 = new QuantityLength(3.0, LengthUnit.FEET);

            Console.WriteLine("Equality Check:");
            Console.WriteLine($"1 YARD == 3 FEET → {q1.Equals(q2)}");

            Console.WriteLine();

            // UC5 Conversion Examples
            Console.WriteLine("Conversion Examples:");

            Console.WriteLine($"convert(1.0, FEET, INCH) → {QuantityLength.Convert(1.0, LengthUnit.FEET, LengthUnit.INCH)}");

            Console.WriteLine($"convert(3.0, YARD, FEET) → {QuantityLength.Convert(3.0, LengthUnit.YARD, LengthUnit.FEET)}");

            Console.WriteLine($"convert(36.0, INCH, YARD) → {QuantityLength.Convert(36.0, LengthUnit.INCH, LengthUnit.YARD)}");

            Console.WriteLine($"convert(1.0, CENTIMETER, INCH) → {QuantityLength.Convert(1.0, LengthUnit.CENTIMETER, LengthUnit.INCH)}");

            Console.WriteLine();
            Console.WriteLine("UC6 Addition Examples:");

            Console.WriteLine($"add(Quantity(1.0, FEET), Quantity(2.0, FEET)) → {QuantityLength.Add(new QuantityLength(1.0, LengthUnit.FEET), new QuantityLength(2.0, LengthUnit.FEET))}");
            Console.WriteLine($"add(Quantity(1.0, FEET), Quantity(12.0, INCH)) → {QuantityLength.Add(new QuantityLength(1.0, LengthUnit.FEET), new QuantityLength(12.0, LengthUnit.INCH))}");
            Console.WriteLine($"add(Quantity(12.0, INCH), Quantity(1.0, FEET)) → {QuantityLength.Add(new QuantityLength(12.0, LengthUnit.INCH), new QuantityLength(1.0, LengthUnit.FEET))}");
        }
    }
}