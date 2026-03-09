using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityUc12Tests
    {
        private const double EPSILON = 1e-6;

        [TestMethod]
        public void testEquality_CrossUnit_Length_ShouldReturnTrue()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testAddition_CrossUnit_ImplicitTarget()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.FEET)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.INCH));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testAddition_ExplicitTarget_Inch()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.FEET)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.INCH), LengthUnit.INCH);

            Assert.AreEqual(LengthUnit.INCH, result.Unit);
            Assert.AreEqual(24.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_SameUnit_FeetMinusFeet()
        {
            var result = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(5.0, LengthUnit.FEET));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(5.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_CrossUnit_FeetMinusInches_Implicit()
        {
            var result = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(9.5, result.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_ExplicitTarget_Inch()
        {
            var result = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH), LengthUnit.INCH);

            Assert.AreEqual(LengthUnit.INCH, result.Unit);
            Assert.AreEqual(114.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_ResultingInNegative()
        {
            var result = new Quantity<LengthUnit>(5.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(10.0, LengthUnit.FEET));

            Assert.AreEqual(-5.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_ResultingInZero()
        {
            var result = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(120.0, LengthUnit.INCH));

            Assert.AreEqual(0.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_WithZeroOperand()
        {
            var result = new Quantity<LengthUnit>(5.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(0.0, LengthUnit.INCH));

            Assert.AreEqual(5.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_NonCommutative()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);

            var left = a.Subtract(b);
            var right = b.Subtract(a);

            Assert.AreEqual(5.0, left.Value, EPSILON);
            Assert.AreEqual(-5.0, right.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_AllMeasurementCategories()
        {
            var length = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(12.0, LengthUnit.INCH));

            var weight = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM)
                .Subtract(new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM));

            var volume = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE)
                .Subtract(new Quantity<VolumeUnit>(500.0, VolumeUnit.MILLILITRE));

            Assert.AreEqual(9.0, length.Value, EPSILON);
            Assert.AreEqual(5.0, weight.Value, EPSILON);
            Assert.AreEqual(4.5, volume.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_NullOperand()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            bool exceptionThrown = false;

            try
            {
                first.Subtract(null!);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void testSubtraction_Immutability()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var second = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);

            var result = first.Subtract(second);

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(8.0, result.Value, EPSILON);
            Assert.AreEqual(10.0, first.Value, EPSILON);
            Assert.AreEqual(2.0, second.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtraction_PrecisionAndRounding()
        {
            var result = new Quantity<LengthUnit>(1.235, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(0.0, LengthUnit.FEET));

            Assert.AreEqual(1.24, result.Value, EPSILON);
        }

        [TestMethod]
        public void testDivision_SameUnit()
        {
            double ratio = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Divide(new Quantity<LengthUnit>(2.0, LengthUnit.FEET));

            Assert.AreEqual(5.0, ratio, EPSILON);
        }

        [TestMethod]
        public void testDivision_CrossUnit()
        {
            double ratio = new Quantity<LengthUnit>(24.0, LengthUnit.INCH)
                .Divide(new Quantity<LengthUnit>(2.0, LengthUnit.FEET));

            Assert.AreEqual(1.0, ratio, EPSILON);
        }

        [TestMethod]
        public void testDivision_RatioCases()
        {
            double greater = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Divide(new Quantity<LengthUnit>(5.0, LengthUnit.FEET));

            double less = new Quantity<LengthUnit>(5.0, LengthUnit.FEET)
                .Divide(new Quantity<LengthUnit>(10.0, LengthUnit.FEET));

            double equal = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Divide(new Quantity<LengthUnit>(120.0, LengthUnit.INCH));

            Assert.AreEqual(2.0, greater, EPSILON);
            Assert.AreEqual(0.5, less, EPSILON);
            Assert.AreEqual(1.0, equal, EPSILON);
        }

        [TestMethod]
        public void testDivision_NonCommutative()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);

            Assert.AreEqual(2.0, a.Divide(b), EPSILON);
            Assert.AreEqual(0.5, b.Divide(a), EPSILON);
        }

        [TestMethod]
        public void testDivision_ByZero()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            bool exceptionThrown = false;

            try
            {
                first.Divide(new Quantity<LengthUnit>(0.0, LengthUnit.FEET));
            }
            catch (ArithmeticException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void testDivision_NullOperand()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            bool exceptionThrown = false;

            try
            {
                first.Divide(null!);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void testDivision_AllMeasurementCategories()
        {
            double length = new Quantity<LengthUnit>(12.0, LengthUnit.INCH)
                .Divide(new Quantity<LengthUnit>(1.0, LengthUnit.FEET));

            double weight = new Quantity<WeightUnit>(2000.0, WeightUnit.GRAM)
                .Divide(new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM));

            double volume = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE)
                .Divide(new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE));

            Assert.AreEqual(1.0, length, EPSILON);
            Assert.AreEqual(2.0, weight, EPSILON);
            Assert.AreEqual(1.0, volume, EPSILON);
        }

        [TestMethod]
        public void testDivision_Immutability()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var second = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);

            double ratio = first.Divide(second);

            Assert.AreEqual(2.0, ratio, EPSILON);
            Assert.AreEqual(10.0, first.Value, EPSILON);
            Assert.AreEqual(5.0, second.Value, EPSILON);
        }

        [TestMethod]
        public void testSubtractionAddition_Inverse()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);

            var result = a.Add(b).Subtract(b);

            Assert.AreEqual(a.Unit, result.Unit);
            Assert.AreEqual(a.Value, result.Value, EPSILON);
        }
    }
}