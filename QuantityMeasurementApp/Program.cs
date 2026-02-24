using System;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
        //feet and inch classes with overridden Equals and GetHashCode methods
        public class Feet
        {
            private readonly double value;

            public Feet(double value)
            {
                this.value = value;
            }

            public override bool Equals(object? obj)
            {
                if (this == obj)
                    return true;

                if (obj == null || this.GetType() != obj.GetType())
                    return false;

                Feet other = (Feet)obj;

                return this.value == other.value;
            }
            // GetHashCode is overridden to maintain consistency with Equals

            public override int GetHashCode()
            {
                return value.GetHashCode();
            }
        }

        // inch class with overridden Equals and GetHashCode methods
        public class Inch
        {
            private readonly double value;

            public Inch(double value)
            {
                this.value = value;
            }

            public override bool Equals(object? obj) // Overriding Equals to compare Inch objects based on their value
            {
                if (this == obj)
                    return true;

                if (obj == null || this.GetType() != obj.GetType())
                    return false;

                Inch other = (Inch)obj;

                return this.value == other.value;
            }

            public override int GetHashCode()
            {
                return value.GetHashCode();
            }
        }

     // Methods to compare feet and inch values

        public static bool CompareFeet(double value1, double value2)
        {
            Feet f1 = new Feet(value1);
            Feet f2 = new Feet(value2);

            return f1.Equals(f2);
        }

        public static bool CompareInch(double value1, double value2)
        {
            Inch i1 = new Inch(value1);
            Inch i2 = new Inch(value2);

            return i1.Equals(i2);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input: 1.0 inch and 1.0 inch");
            Console.WriteLine("Output: Equal (" +
                QuantityMeasurementApp.CompareInch(1.0, 1.0) + ")");

            Console.WriteLine();

            Console.WriteLine("Input: 1.0 ft and 1.0 ft");
            Console.WriteLine("Output: Equal (" +
                QuantityMeasurementApp.CompareFeet(1.0, 1.0) + ")");
        }
    }
}