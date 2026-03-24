using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.QuantityTests
{
    [TestFixture]
    public class QuantityDivisionTests
    {
        // Same Unit Division Tests
        [Test]
        public void TestDivision_SameUnit_FeetDividedByFeet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(5.0));
        }

        [Test]
        public void TestDivision_SameUnit_LitreDividedByLitre()
        {
            var q1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(2.0));
        }

        [Test]
        public void TestDivision_SameUnit_KilogramDividedByKilogram()
        {
            var q1 = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(2.0, WeightUnit.KILOGRAM);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(5.0));
        }

        // Cross-Unit Division Tests
        [Test]
        public void TestDivision_CrossUnit_FeetDividedByInches()
        {
            var q1 = new Quantity<LengthUnit>(24.0, LengthUnit.INCHES);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(1.0));
        }

        [Test]
        public void TestDivision_CrossUnit_KilogramDividedByGram()
        {
            var q1 = new Quantity<WeightUnit>(2.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(2000.0, WeightUnit.GRAM);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(1.0));
        }

        [Test]
        public void TestDivision_CrossUnit_LitreDividedByMillilitre()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(1.0));
        }

        [Test]
        public void TestDivision_CrossUnit_GramDividedByKilogram()
        {
            var q1 = new Quantity<WeightUnit>(2000.0, WeightUnit.GRAM);
            var q2 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(2.0));
        }

        // Ratio > 1.0 Tests
        [Test]
        public void TestDivision_RatioGreaterThanOne_Feet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(5.0));
            Assert.That(result, Is.GreaterThan(1.0));
        }

        [Test]
        public void TestDivision_RatioGreaterThanOne_Kilogram()
        {
            var q1 = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(3.0, WeightUnit.KILOGRAM);
            var result = q1.Divide(q2);
            Assert.That(Math.Abs(result - 3.33), Is.LessThan(0.01));
            Assert.That(result, Is.GreaterThan(1.0));
        }

        // Ratio < 1.0 Tests
        [Test]
        public void TestDivision_RatioLessThanOne_Feet()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(0.5));
            Assert.That(result, Is.LessThan(1.0));
        }

        [Test]
        public void TestDivision_RatioLessThanOne_Kilogram()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(0.1));
            Assert.That(result, Is.LessThan(1.0));
        }

        [Test]
        public void TestDivision_RatioLessThanOne_Volume()
        {
            var q1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(0.5));
            Assert.That(result, Is.LessThan(1.0));
        }

        // Ratio = 1.0 Tests (Equivalence Detection)
        [Test]
        public void TestDivision_RatioEqualToOne_Feet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(1.0));
        }

        [Test]
        public void TestDivision_RatioEqualToOne_CrossUnit()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(1.0));
        }

        [Test]
        public void TestDivision_RatioEqualToOne_Kilogram()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(1.0));
        }

        // Non-Commutativity Tests
        [Test]
        public void TestDivision_NonCommutative_Feet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var result1 = q1.Divide(q2);
            var result2 = q2.Divide(q1);
            Assert.That(result1, Is.EqualTo(2.0));
            Assert.That(result2, Is.EqualTo(0.5));
            Assert.That(result1, Is.Not.EqualTo(result2));
        }

        [Test]
        public void TestDivision_NonCommutative_Weight()
        {
            var q1 = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(5.0, WeightUnit.KILOGRAM);
            var result1 = q1.Divide(q2);
            var result2 = q2.Divide(q1);
            Assert.That(result1, Is.EqualTo(2.0));
            Assert.That(result2, Is.EqualTo(0.5));
        }

        // Division by Zero Tests
        [Test]
        public void TestDivision_ByZero_ThrowsException()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(0.0, LengthUnit.FEET);
            var ex = Assert.Throws<ArithmeticException>(() => q1.Divide(q2));
            Assert.That(ex.Message, Does.Contain("divide by zero"));
        }

        [Test]
        public void TestDivision_ByZeroWeight_ThrowsException()
        {
            var q1 = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(0.0, WeightUnit.KILOGRAM);
            var ex = Assert.Throws<ArithmeticException>(() => q1.Divide(q2));
            Assert.That(ex.Message, Does.Contain("divide by zero"));
        }

        [Test]
        public void TestDivision_ByZeroVolume_ThrowsException()
        {
            var q1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(0.0, VolumeUnit.LITRE);
            var ex = Assert.Throws<ArithmeticException>(() => q1.Divide(q2));
            Assert.That(ex.Message, Does.Contain("divide by zero"));
        }

        // Very Large Ratio Tests
        [Test]
        public void TestDivision_WithLargeRatio()
        {
            var q1 = new Quantity<WeightUnit>(1e6, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(1e6));
        }

        // Very Small Ratio Tests
        [Test]
        public void TestDivision_WithSmallRatio()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(1e6, WeightUnit.KILOGRAM);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(1e-6));
        }

        // Null Operand Tests
        [Test]
        public void TestDivision_NullOperand_ThrowsException()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type
            var ex = Assert.Throws<ArgumentException>(() => q1.Divide(null));
#pragma warning restore CS8625
            Assert.That(ex.Message, Does.Contain("null quantity"));
        }

        // Cross-Category Prevention: Prevented by compile-time type safety
        // The generic type parameter U ensures only quantities of the same unit type can be operated on
        // Different measurement categories (Length, Weight, Volume) have different unit enum types,
        // so it's impossible to call Divide(otherQuantity) with a different category at compile-time

        // Negative Operand Tests
        [Test]
        public void TestDivision_WithNegativeDividend()
        {
            var q1 = new Quantity<LengthUnit>(-10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(-5.0));
        }

        [Test]
        public void TestDivision_WithNegativeDivisor()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(-2.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(-5.0));
        }

        // Immutability Tests
        [Test]
        public void TestDivision_Immutability()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(q1.Value, Is.EqualTo(10.0), "First operand should not be modified");
            Assert.That(q2.Value, Is.EqualTo(2.0), "Second operand should not be modified");
            Assert.That(result, Is.EqualTo(5.0), "Result should be correct");
        }

        // All Measurement Categories Tests
        [Test]
        public void TestDivision_AllCategories_Length()
        {
            var q1 = new Quantity<LengthUnit>(20.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(4.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(5.0));
        }

        [Test]
        public void TestDivision_AllCategories_Weight()
        {
            var q1 = new Quantity<WeightUnit>(15.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(3.0, WeightUnit.KILOGRAM);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(5.0));
        }

        [Test]
        public void TestDivision_AllCategories_Volume()
        {
            var q1 = new Quantity<VolumeUnit>(20.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(4.0, VolumeUnit.LITRE);
            var result = q1.Divide(q2);
            Assert.That(result, Is.EqualTo(5.0));
        }

        // Dimensionless Result Property Tests
        [Test]
        public void TestDivision_ReturnsDimensionlessScalar_Feet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            // Verify result is a double (dimensionless scalar)
            Assert.That(result, Is.EqualTo(2.0));
        }

        // Associativity Test (Non-Associative Property)
        [Test]
        public void TestDivision_NonAssociative()
        {
            var q1 = new Quantity<LengthUnit>(24.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var q3 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);

            // (24 ÷ 2) ÷ 2 = 12 ÷ 2 = 6
            var division1 = q1.Divide(q2);  // 24 / 2 = 12
            var division2 = q2.Divide(q3);  // 2 / 2 = 1
            
            // Ratios are different
            Assert.That(division1, Is.EqualTo(12.0));
            Assert.That(division2, Is.EqualTo(1.0));
            Assert.That(division1, Is.Not.EqualTo(division2));
        }

        // Precision Handling Tests
        [Test]
        public void TestDivision_PrecisionHandling()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);
            var result = q1.Divide(q2);
            Assert.That(Math.Abs(result - 3.3333333), Is.LessThan(0.001));
        }

        // Integration Tests
        [Test]
        public void TestSubtraction_And_Division_Integration()
        {
            var q1 = new Quantity<LengthUnit>(15.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var difference = q1.Subtract(q2);
            var ratio = q1.Divide(q2);
            Assert.That(difference.Value, Is.EqualTo(10.0));
            Assert.That(ratio, Is.EqualTo(3.0));
        }

        [Test]
        public void TestAddition_And_Division_Integration()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var sum = q1.Add(q2);
            var ratio = q1.Divide(q2);
            Assert.That(sum.Value, Is.EqualTo(15.0));
            Assert.That(ratio, Is.EqualTo(0.5));
        }
    }
}
