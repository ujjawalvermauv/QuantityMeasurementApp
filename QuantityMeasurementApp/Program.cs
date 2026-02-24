using System;

namespace QuantityMeasurementApp
{
    // enum for supported length units
    public enum LengthUnit
    {
        FEET,
        INCH
    }

    //entity class to represent a quantity with a value and unit
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        private const double INCH_TO_FEET = 1.0 / 12.0;

        public QuantityLength(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        // Convert everything to FEET (base unit)
        private double ToFeet()
        {
            return unit switch
            {
                LengthUnit.FEET => value,
                LengthUnit.INCH => value * INCH_TO_FEET,
                _ => throw new ArgumentException("Unsupported Unit")
            };
        }

        public override bool Equals(object? obj) // override Equals to compare quantities based on their value in feet
        {
            if (this == obj)
                return true;

            if (obj == null || this.GetType() != obj.GetType())
                return false;

            QuantityLength other = (QuantityLength)obj;

            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCH);

            Console.WriteLine("Input: Quantity(1.0, FEET) and Quantity(12.0, INCH)");
            Console.WriteLine("Output: Equal (" + q1.Equals(q2) + ")");

            Console.WriteLine();

            var q3 = new QuantityLength(1.0, LengthUnit.INCH);
            var q4 = new QuantityLength(1.0, LengthUnit.INCH);

            Console.WriteLine("Input: Quantity(1.0, INCH) and Quantity(1.0, INCH)");
            Console.WriteLine("Output: Equal (" + q3.Equals(q4) + ")");
        }
    }
}