using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthEqualityTest
    {
        // ---------------- UC1 & UC2 ----------------

        [TestMethod]
        public void GivenFeetToFeet_SameValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenFeetDifferentValue_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(2.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(q2));
        }

        // ---------------- UC2 ----------------

        [TestMethod]
        public void GivenInchToInch_SameValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.INCH);
            var q2 = new QuantityLength(1.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenInchDifferentValue_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.INCH);
            var q2 = new QuantityLength(2.0, LengthUnit.INCH);

            Assert.IsFalse(q1.Equals(q2));
        }

        // ---------------- UC3 Cross Unit ----------------

        [TestMethod]
        public void GivenFeetToInch_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenInchToFeet_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(12.0, LengthUnit.INCH);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        // ---------------- UC4 Yard Support ----------------

        [TestMethod]
        public void GivenYardToFeet_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.YARD);
            var q2 = new QuantityLength(3.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenYardToInch_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.YARD);
            var q2 = new QuantityLength(36.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        // ---------------- UC4 Centimeter Support ----------------

        [TestMethod]
        public void GivenCentimeterToInch_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.CENTIMETER);
            var q2 = new QuantityLength(0.393701, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenCentimeterToFeet_NonEquivalent_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.CENTIMETER);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(q2));
        }

        // ---------------- Object Behavior ----------------

        [TestMethod]
        public void GivenNullComparison_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsFalse(q1.Equals(null));
        }

        [TestMethod]
        public void GivenSameReference_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsTrue(q1.Equals(q1));
        }
    }

    [TestClass]
    public class LengthUnitStandaloneConversionTest
    {
        private const double EPSILON = 1e-6;

        [TestMethod]
        public void testConvertToBaseUnit_FeetToFeet()
        {
            Assert.AreEqual(5.0, LengthUnit.FEET.ConvertToBaseUnit(5.0), EPSILON);
        }

        [TestMethod]
        public void testConvertToBaseUnit_InchesToFeet()
        {
            Assert.AreEqual(1.0, LengthUnit.INCH.ConvertToBaseUnit(12.0), EPSILON);
        }

        [TestMethod]
        public void testConvertToBaseUnit_YardsToFeet()
        {
            Assert.AreEqual(3.0, LengthUnit.YARD.ConvertToBaseUnit(1.0), EPSILON);
        }

        [TestMethod]
        public void testConvertToBaseUnit_CentimetersToFeet()
        {
            Assert.AreEqual(1.0, LengthUnit.CENTIMETER.ConvertToBaseUnit(30.48), 1e-4);
        }

        [TestMethod]
        public void testConvertFromBaseUnit_FeetToInches()
        {
            Assert.AreEqual(12.0, LengthUnit.INCH.ConvertFromBaseUnit(1.0), EPSILON);
        }

        [TestMethod]
        public void testConvertFromBaseUnit_FeetToYards()
        {
            Assert.AreEqual(1.0, LengthUnit.YARD.ConvertFromBaseUnit(3.0), EPSILON);
        }

        [TestMethod]
        public void testConvertFromBaseUnit_FeetToCentimeters()
        {
            Assert.AreEqual(30.48, LengthUnit.CENTIMETER.ConvertFromBaseUnit(1.0), 1e-3);
        }
    }

    [TestClass]
    public class QuantityLengthAdditionTest
    {
        private const double EPSILON = 1e-6;

        [TestMethod]
        public void testAddition_SameUnit_FeetPlusFeet()
        {
            var result = QuantityLength.Add(
                new QuantityLength(1.0, LengthUnit.FEET),
                new QuantityLength(2.0, LengthUnit.FEET));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_SameUnit_InchPlusInch()
        {
            var result = QuantityLength.Add(
                new QuantityLength(6.0, LengthUnit.INCH),
                new QuantityLength(6.0, LengthUnit.INCH));

            Assert.AreEqual(LengthUnit.INCH, result.Unit);
            Assert.AreEqual(12.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_CrossUnit_FeetPlusInches()
        {
            var result = QuantityLength.Add(
                new QuantityLength(1.0, LengthUnit.FEET),
                new QuantityLength(12.0, LengthUnit.INCH));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_CrossUnit_InchPlusFeet()
        {
            var result = QuantityLength.Add(
                new QuantityLength(12.0, LengthUnit.INCH),
                new QuantityLength(1.0, LengthUnit.FEET));

            Assert.AreEqual(LengthUnit.INCH, result.Unit);
            Assert.AreEqual(24.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_CrossUnit_YardPlusFeet()
        {
            var result = QuantityLength.Add(
                new QuantityLength(1.0, LengthUnit.YARD),
                new QuantityLength(3.0, LengthUnit.FEET));

            Assert.AreEqual(LengthUnit.YARD, result.Unit);
            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_CrossUnit_CentimeterPlusInch()
        {
            var result = QuantityLength.Add(
                new QuantityLength(2.54, LengthUnit.CENTIMETER),
                new QuantityLength(1.0, LengthUnit.INCH));

            Assert.AreEqual(LengthUnit.CENTIMETER, result.Unit);
            Assert.AreEqual(5.08, result.Value, 1e-4);
        }

        [TestMethod]
        public void testAddition_Commutativity_WithTargetUnit()
        {
            var first = new QuantityLength(1.0, LengthUnit.FEET);
            var second = new QuantityLength(12.0, LengthUnit.INCH);

            var left = QuantityLength.Add(first, second, LengthUnit.FEET);
            var right = QuantityLength.Add(second, first, LengthUnit.FEET);

            Assert.AreEqual(left.Unit, right.Unit);
            Assert.AreEqual(left.Value, right.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_WithZero()
        {
            var result = QuantityLength.Add(
                new QuantityLength(5.0, LengthUnit.FEET),
                new QuantityLength(0.0, LengthUnit.INCH));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(5.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_NegativeValues()
        {
            var result = QuantityLength.Add(
                new QuantityLength(5.0, LengthUnit.FEET),
                new QuantityLength(-2.0, LengthUnit.FEET));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_NullSecondOperand()
        {
            var first = new QuantityLength(1.0, LengthUnit.FEET);
            bool exceptionThrown = false;

            try
            {
                QuantityLength.Add(first, null!);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected ArgumentNullException was not thrown.");
        }

        [TestMethod]
        public void testAddition_LargeValues()
        {
            var result = QuantityLength.Add(
                new QuantityLength(1e6, LengthUnit.FEET),
                new QuantityLength(1e6, LengthUnit.FEET));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(2e6, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_SmallValues()
        {
            var result = QuantityLength.Add(
                new QuantityLength(0.001, LengthUnit.FEET),
                new QuantityLength(0.002, LengthUnit.FEET));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(0.003, result.Value, EPSILON);
        }
    }

    [TestClass]
    public class QuantityWeightEqualityTest
    {
        private const double EPSILON = 1e-4;

        [TestMethod]
        public void testEquality_KilogramToKilogram_SameValue()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var q2 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_KilogramToKilogram_DifferentValue()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var q2 = new QuantityWeight(2.0, WeightUnit.KILOGRAM);

            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_GramToGram_SameValue()
        {
            var q1 = new QuantityWeight(1000.0, WeightUnit.GRAM);
            var q2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_PoundToPound_SameValue()
        {
            var q1 = new QuantityWeight(2.0, WeightUnit.POUND);
            var q2 = new QuantityWeight(2.0, WeightUnit.POUND);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_KilogramToGram_EquivalentValue()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var q2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_GramToKilogram_EquivalentValue()
        {
            var q1 = new QuantityWeight(1000.0, WeightUnit.GRAM);
            var q2 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_KilogramToPound_EquivalentValue()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var q2 = new QuantityWeight(2.20462, WeightUnit.POUND);

            Assert.IsTrue(q1.Equals(q2), "1 kg should equal 2.20462 lbs within epsilon");
        }

        [TestMethod]
        public void testEquality_WeightVsLength_Incompatible()
        {
            var weight = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var length = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsFalse(weight.Equals(length));
        }

        [TestMethod]
        public void testEquality_NullComparison()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Assert.IsFalse(q1.Equals(null));
        }

        [TestMethod]
        public void testEquality_SameReference()
        {
            var q1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Assert.IsTrue(q1.Equals(q1));
        }

        [TestMethod]
        public void testEquality_NegativeWeight()
        {
            var q1 = new QuantityWeight(-1.0, WeightUnit.KILOGRAM);
            var q2 = new QuantityWeight(-1000.0, WeightUnit.GRAM);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_ZeroValue()
        {
            var q1 = new QuantityWeight(0.0, WeightUnit.KILOGRAM);
            var q2 = new QuantityWeight(0.0, WeightUnit.GRAM);

            Assert.IsTrue(q1.Equals(q2));
        }
    }

    [TestClass]
    public class QuantityWeightConversionTest
    {
        private const double EPSILON = 1e-4;

        [TestMethod]
        public void testConversion_KilogramToGram()
        {
            var result = QuantityWeight.Convert(1.0, WeightUnit.KILOGRAM, WeightUnit.GRAM);
            Assert.AreEqual(1000.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_GramToKilogram()
        {
            var result = QuantityWeight.Convert(1000.0, WeightUnit.GRAM, WeightUnit.KILOGRAM);
            Assert.AreEqual(1.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_PoundToKilogram()
        {
            var result = QuantityWeight.Convert(2.20462, WeightUnit.POUND, WeightUnit.KILOGRAM);
            Assert.AreEqual(1.0, result, 1e-4);
        }

        [TestMethod]
        public void testConversion_KilogramToPound()
        {
            var result = QuantityWeight.Convert(1.0, WeightUnit.KILOGRAM, WeightUnit.POUND);
            Assert.AreEqual(2.20462, result, 1e-4);
        }

        [TestMethod]
        public void testConversion_SameUnit()
        {
            var result = QuantityWeight.Convert(5.0, WeightUnit.KILOGRAM, WeightUnit.KILOGRAM);
            Assert.AreEqual(5.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_RoundTrip()
        {
            var original = 1.5;
            var converted = QuantityWeight.Convert(original, WeightUnit.KILOGRAM, WeightUnit.GRAM);
            var roundTrip = QuantityWeight.Convert(converted, WeightUnit.GRAM, WeightUnit.KILOGRAM);
            Assert.AreEqual(original, roundTrip, EPSILON);
        }
    }

    [TestClass]
    public class QuantityWeightAdditionTest
    {
        private const double EPSILON = 1e-4;

        [TestMethod]
        public void testAddition_SameUnit_KilogramPlusKilogram()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(1.0, WeightUnit.KILOGRAM),
                new QuantityWeight(2.0, WeightUnit.KILOGRAM));

            Assert.AreEqual(WeightUnit.KILOGRAM, result.Unit);
            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_SameUnit_GramPlusGram()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(500.0, WeightUnit.GRAM),
                new QuantityWeight(500.0, WeightUnit.GRAM));

            Assert.AreEqual(WeightUnit.GRAM, result.Unit);
            Assert.AreEqual(1000.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_CrossUnit_KilogramPlusGram()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(1.0, WeightUnit.KILOGRAM),
                new QuantityWeight(1000.0, WeightUnit.GRAM));

            Assert.AreEqual(WeightUnit.KILOGRAM, result.Unit);
            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_ExplicitTargetUnit_Gram()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(1.0, WeightUnit.KILOGRAM),
                new QuantityWeight(1000.0, WeightUnit.GRAM),
                WeightUnit.GRAM);

            Assert.AreEqual(WeightUnit.GRAM, result.Unit);
            Assert.AreEqual(2000.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_WithZero()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(5.0, WeightUnit.KILOGRAM),
                new QuantityWeight(0.0, WeightUnit.GRAM));

            Assert.AreEqual(WeightUnit.KILOGRAM, result.Unit);
            Assert.AreEqual(5.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_NegativeValues()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(5.0, WeightUnit.KILOGRAM),
                new QuantityWeight(-2000.0, WeightUnit.GRAM));

            Assert.AreEqual(WeightUnit.KILOGRAM, result.Unit);
            Assert.AreEqual(3.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_LargeValues()
        {
            var result = QuantityWeight.Add(
                new QuantityWeight(1e6, WeightUnit.KILOGRAM),
                new QuantityWeight(1e6, WeightUnit.KILOGRAM));

            Assert.AreEqual(WeightUnit.KILOGRAM, result.Unit);
            Assert.AreEqual(2e6, result.Value, EPSILON);
        }
    }
}