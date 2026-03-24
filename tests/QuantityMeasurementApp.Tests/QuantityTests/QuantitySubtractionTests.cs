using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.QuantityTests
{
    [TestFixture]
    public class QuantitySubtractionTests
    {
        // Same Unit Subtraction Tests
        [Test]
        public void TestSubtraction_SameUnit_FeetMinusFeet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(5.0));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void TestSubtraction_SameUnit_LitreMinusLitre()
        {
            var q1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(3.0, VolumeUnit.LITRE);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(7.0));
            Assert.That(result.Unit, Is.EqualTo(VolumeUnit.LITRE));
        }

        [Test]
        public void TestSubtraction_SameUnit_KilogramMinusKilogram()
        {
            var q1 = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(3.0, WeightUnit.KILOGRAM);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(7.0));
            Assert.That(result.Unit, Is.EqualTo(WeightUnit.KILOGRAM));
        }

        // Cross-Unit Subtraction Tests (Implicit Target Unit = First Operand's Unit)
        [Test]
        public void TestSubtraction_CrossUnit_FeetMinusInches()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.INCHES);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(9.5));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void TestSubtraction_CrossUnit_InchesMinusFeet()
        {
            var q1 = new Quantity<LengthUnit>(120.0, LengthUnit.INCHES);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(60.0));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.INCHES));
        }

        [Test]
        public void TestSubtraction_CrossUnit_KilogramMinusGram()
        {
            var q1 = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(5.0));
            Assert.That(result.Unit, Is.EqualTo(WeightUnit.KILOGRAM));
        }

        [Test]
        public void TestSubtraction_CrossUnit_LitreMinusMillilitre()
        {
            var q1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(500.0, VolumeUnit.MILLILITRE);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(4.5));
            Assert.That(result.Unit, Is.EqualTo(VolumeUnit.LITRE));
        }

        // Explicit Target Unit Tests
        [Test]
        public void TestSubtraction_ExplicitTargetUnit_Feet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.INCHES);
            var result = q1.Subtract(q2, LengthUnit.FEET);
            Assert.That(result.Value, Is.EqualTo(9.5));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void TestSubtraction_ExplicitTargetUnit_Inches()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.INCHES);
            var result = q1.Subtract(q2, LengthUnit.INCHES);
            Assert.That(result.Value, Is.EqualTo(114.0));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.INCHES));
        }

        [Test]
        public void TestSubtraction_ExplicitTargetUnit_Millilitre()
        {
            var q1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);
            var result = q1.Subtract(q2, VolumeUnit.MILLILITRE);
            Assert.That(result.Value, Is.EqualTo(3000.0));
            Assert.That(result.Unit, Is.EqualTo(VolumeUnit.MILLILITRE));
        }

        [Test]
        public void TestSubtraction_ExplicitTargetUnit_Gram()
        {
            var q1 = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM);
            var result = q1.Subtract(q2, WeightUnit.GRAM);
            Assert.That(result.Value, Is.EqualTo(5000.0));
            Assert.That(result.Unit, Is.EqualTo(WeightUnit.GRAM));
        }

        // Negative Result Tests
        [Test]
        public void TestSubtraction_ResultingInNegative_Feet()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(-5.0));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void TestSubtraction_ResultingInNegative_Kilogram()
        {
            var q1 = new Quantity<WeightUnit>(2.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(5.0, WeightUnit.KILOGRAM);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(-3.0));
            Assert.That(result.Unit, Is.EqualTo(WeightUnit.KILOGRAM));
        }

        // Zero Result Tests
        [Test]
        public void TestSubtraction_ResultingInZero_Feet()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(120.0, LengthUnit.INCHES);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(0.0));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void TestSubtraction_ResultingInZero_Litre()
        {
            var q1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(0.0));
            Assert.That(result.Unit, Is.EqualTo(VolumeUnit.LITRE));
        }

        // Identity Element Tests (Zero Operand)
        [Test]
        public void TestSubtraction_WithZeroOperand_Feet()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(0.0, LengthUnit.INCHES);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(5.0));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        // Negative Operand Tests
        [Test]
        public void TestSubtraction_WithNegativeValues()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(-2.0, LengthUnit.FEET);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(7.0));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        // Non-Commutativity Tests
        [Test]
        public void TestSubtraction_NonCommutative()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var result1 = q1.Subtract(q2);
            var result2 = q2.Subtract(q1);
            Assert.That(result1.Value, Is.EqualTo(5.0));
            Assert.That(result2.Value, Is.EqualTo(-5.0));
        }

        // Large Value Tests
        [Test]
        public void TestSubtraction_WithLargeValues()
        {
            var q1 = new Quantity<WeightUnit>(1e6, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(5e5, WeightUnit.KILOGRAM);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(5e5));
            Assert.That(result.Unit, Is.EqualTo(WeightUnit.KILOGRAM));
        }

        // Small Value Tests
        [Test]
        public void TestSubtraction_WithSmallValues()
        {
            var q1 = new Quantity<LengthUnit>(0.001, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(0.0005, LengthUnit.FEET);
            var result = q1.Subtract(q2);
            // Due to rounding to 2 decimal places, 0.0005 becomes 0.00
            Assert.That(result.Value, Is.EqualTo(0.00));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        // Null Operand Tests
        [Test]
        public void TestSubtraction_NullOperand_ThrowsException()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type
            var ex = Assert.Throws<ArgumentException>(() => q1.Subtract(null));
#pragma warning restore CS8625
            Assert.That(ex.Message, Does.Contain("null quantity"));
        }

        // Cross-Category Prevention: Prevented by compile-time type safety
        // The generic type parameter U ensures only quantities of the same unit type can be operated on
        // Different measurement categories (Length, Weight, Volume) have different unit enum types,
        // so it's impossible to call Subtract(otherQuantity) with a different category at compile-time

        // Immutability Tests
        [Test]
        public void TestSubtraction_Immutability()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var result = q1.Subtract(q2);
            Assert.That(q1.Value, Is.EqualTo(10.0), "First operand should not be modified");
            Assert.That(q2.Value, Is.EqualTo(5.0), "Second operand should not be modified");
            Assert.That(result.Value, Is.EqualTo(5.0), "Result should be correct");
        }

        // Chained Operations Test
        [Test]
        public void TestSubtraction_ChainedOperations()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var q3 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var result = q1.Subtract(q2).Subtract(q3);
            Assert.That(result.Value, Is.EqualTo(7.0));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        // All Measurement Categories Tests
        [Test]
        public void TestSubtraction_AllCategories_Length()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(5.0));
        }

        [Test]
        public void TestSubtraction_AllCategories_Weight()
        {
            var q1 = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(3.0, WeightUnit.KILOGRAM);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(7.0));
        }

        [Test]
        public void TestSubtraction_AllCategories_Volume()
        {
            var q1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(4.0, VolumeUnit.LITRE);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(6.0));
        }

        // Precision and Rounding Tests
        [Test]
        public void TestSubtraction_PrecisionAndRounding()
        {
            var q1 = new Quantity<LengthUnit>(10.123, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5.456, LengthUnit.FEET);
            var result = q1.Subtract(q2);
            Assert.That(result.Value, Is.EqualTo(4.67)); // Rounded to 2 decimal places
        }

        // Integration with Addition (Inverse Relationship)
        [Test]
        public void TestSubtraction_Addition_Inverse()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            var result = q1.Subtract(q2).Add(q2);
            Assert.That(result.Value, Is.EqualTo(q1.Value));
            Assert.That(result.Unit, Is.EqualTo(q1.Unit));
        }
    }
}
