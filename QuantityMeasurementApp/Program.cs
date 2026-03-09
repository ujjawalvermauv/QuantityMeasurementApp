using System;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var q1 = new QuantityLength(1.0, LengthUnit.YARD);
            var q2 = new QuantityLength(3.0, LengthUnit.FEET);

            Console.WriteLine("Equality Check:");
            Console.WriteLine($"1 YARD == 3 FEET ? {q1.Equals(q2)}");

            Console.WriteLine();
            Console.WriteLine("Conversion Examples:");
            Console.WriteLine($"convert(1.0, FEET, INCH) ? {Quantity<LengthUnit>.Convert(1.0, LengthUnit.FEET, LengthUnit.INCH)}");
            Console.WriteLine($"convert(3.0, YARD, FEET) ? {Quantity<LengthUnit>.Convert(3.0, LengthUnit.YARD, LengthUnit.FEET)}");
            Console.WriteLine($"convert(36.0, INCH, YARD) ? {Quantity<LengthUnit>.Convert(36.0, LengthUnit.INCH, LengthUnit.YARD)}");

            Console.WriteLine();
            Console.WriteLine("UC8 Unit Conversion Delegation:");
            Console.WriteLine($"LengthUnit.FEET.ConvertToBaseUnit(12.0) ? {LengthUnit.FEET.ConvertToBaseUnit(12.0)}");
            Console.WriteLine($"LengthUnit.INCH.ConvertToBaseUnit(12.0) ? {LengthUnit.INCH.ConvertToBaseUnit(12.0)}");

            Console.WriteLine();
            Console.WriteLine("Addition Examples:");
            Console.WriteLine($"add(Quantity(1.0, FEET), Quantity(12.0, INCH), FEET) ? {Quantity<LengthUnit>.Add(new QuantityLength(1.0, LengthUnit.FEET), new QuantityLength(12.0, LengthUnit.INCH), LengthUnit.FEET)}");
            Console.WriteLine($"add(Quantity(1.0, FEET), Quantity(12.0, INCH), YARD) ? {Quantity<LengthUnit>.Add(new QuantityLength(1.0, LengthUnit.FEET), new QuantityLength(12.0, LengthUnit.INCH), LengthUnit.YARD)}");

            Console.WriteLine();
            Console.WriteLine("UC9 Weight Equality Comparisons:");
            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);
            Console.WriteLine($"Quantity(1.0, KILOGRAM).equals(Quantity(1000.0, GRAM)) ? {w1.Equals(w2)}");
            
            var w3 = new QuantityWeight(2.20462, WeightUnit.POUND);
            var w4 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Console.WriteLine($"Quantity(2.20462, POUND).equals(Quantity(1.0, KILOGRAM)) ? {w3.Equals(w4)}");

            Console.WriteLine();
            Console.WriteLine("UC9 Weight Conversions:");
            var wkg = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Console.WriteLine($"Quantity(1.0, KILOGRAM).convertTo(GRAM) ? {wkg.ConvertTo(WeightUnit.GRAM)}");
            
            var wpound = new QuantityWeight(2.20462, WeightUnit.POUND);
            Console.WriteLine($"Quantity(2.20462, POUND).convertTo(KILOGRAM) ? {wpound.ConvertTo(WeightUnit.KILOGRAM)}");

            Console.WriteLine();
            Console.WriteLine("UC9 Weight Addition:");
            var wa1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var wa2 = new QuantityWeight(1000.0, WeightUnit.GRAM);
            Console.WriteLine($"add(Quantity(1.0, KILOGRAM), Quantity(1000.0, GRAM)) ? {Quantity<WeightUnit>.Add(wa1, wa2, wa1.Unit)}");
            
            var wa3 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var wa4 = new QuantityWeight(1000.0, WeightUnit.GRAM);
            Console.WriteLine($"add(Quantity(1.0, KILOGRAM), Quantity(1000.0, GRAM), GRAM) ? {Quantity<WeightUnit>.Add(wa3, wa4, WeightUnit.GRAM)}");

            Console.WriteLine();
            Console.WriteLine("UC10 Generic Quantity Examples:");
            var length1 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var length2 = new Quantity<LengthUnit>(60.0, LengthUnit.INCH);
            Console.WriteLine($"Quantity<LengthUnit>(5.0, FEET).equals(Quantity<LengthUnit>(60.0, INCH)) ? {length1.Equals(length2)}");
            
            var weight1 = new Quantity<WeightUnit>(2.0, WeightUnit.KILOGRAM);
            var weight2 = new Quantity<WeightUnit>(2000.0, WeightUnit.GRAM);
            Console.WriteLine($"Quantity<WeightUnit>(2.0, KILOGRAM).equals(Quantity<WeightUnit>(2000.0, GRAM)) ? {weight1.Equals(weight2)}");

            Console.WriteLine();
            Console.WriteLine("UC11 Volume Equality, Conversion, and Addition:");
            var volume1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var volume2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Console.WriteLine($"Quantity<VolumeUnit>(1.0, LITRE).equals(Quantity<VolumeUnit>(1000.0, MILLILITRE)) ? {volume1.Equals(volume2)}");

            var volumeInGallon = volume1.ConvertTo(VolumeUnit.GALLON);
            Console.WriteLine($"Quantity<VolumeUnit>(1.0, LITRE).convertTo(GALLON) ? {volumeInGallon}");

            var volumeSum = Quantity<VolumeUnit>.Add(volume1, new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON), VolumeUnit.LITRE);
            Console.WriteLine($"add(Quantity(1.0, LITRE), Quantity(1.0, GALLON), LITRE) ? {volumeSum}");
        }
    }
}
