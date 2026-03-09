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
}