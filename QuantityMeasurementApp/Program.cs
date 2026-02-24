using System;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
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

            public override int GetHashCode()
            {
                return value.GetHashCode();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var feet1 = new QuantityMeasurementApp.Feet(1.0);
            var feet2 = new QuantityMeasurementApp.Feet(1.0);

            Console.WriteLine("Input: 1.0 ft and 1.0 ft");
            Console.WriteLine("Output: Equal (" + feet1.Equals(feet2) + ")");
        }
    }
}