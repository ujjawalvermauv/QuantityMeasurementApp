using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityUc13Tests_DRYAndCentralization
    {
        private const double EPSILON = 1e-6;

        [TestMethod]
        public void testValidation_NullOperand_ConsistentAcrossOperations()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            bool addThrows = false, subtractThrows = false, divideThrows = false;

            try { first.Add(null!); } catch (ArgumentNullException) { addThrows = true; }
            try { first.Subtract(null!); } catch (ArgumentNullException) { subtractThrows = true; }
            try { first.Divide(null!); } catch (ArgumentNullException) { divideThrows = true; }

            Assert.IsTrue(addThrows);
            Assert.IsTrue(subtractThrows);
            Assert.IsTrue(divideThrows);
        }

        [TestMethod]
        public void testValidation_InvalidTargetUnit_AddSubtractReject()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var second = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            bool addThrows = false, subtractThrows = false;

            try { first.Add(second, (LengthUnit)999); }
            catch (ArgumentException) { addThrows = true; }

            try { first.Subtract(second, (LengthUnit)999); }
            catch (ArgumentException) { subtractThrows = true; }

            Assert.IsTrue(addThrows);
            Assert.IsTrue(subtractThrows);
        }

        [TestMethod]
        public void testValidation_FiniteValue_EnforcedByConstructor()
        {
            bool nanThrows = false;
            bool infThrows = false;

            try { _ = new Quantity<LengthUnit>(double.NaN, LengthUnit.FEET); }
            catch (ArgumentException) { nanThrows = true; }

            try { _ = new Quantity<LengthUnit>(double.PositiveInfinity, LengthUnit.FEET); }
            catch (ArgumentException) { infThrows = true; }

            Assert.IsTrue(nanThrows);
            Assert.IsTrue(infThrows);
        }

        [TestMethod]
        public void testUC12_AddBehaviorPreserved()
        {
            var result = new Quantity<LengthUnit>(1.0, LengthUnit.FEET)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.INCH));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(2.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testUC12_SubtractBehaviorPreserved()
        {
            var result = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH));

            Assert.AreEqual(LengthUnit.FEET, result.Unit);
            Assert.AreEqual(9.5, result.Value, EPSILON);
        }

        [TestMethod]
        public void testUC12_DivideBehaviorPreserved()
        {
            double ratio = new Quantity<LengthUnit>(24.0, LengthUnit.INCH)
                .Divide(new Quantity<LengthUnit>(2.0, LengthUnit.FEET));

            Assert.AreEqual(1.0, ratio, EPSILON);
        }

        [TestMethod]
        public void testRounding_AddSubtract_TwoDecimalPlaces()
        {
            var result = new Quantity<LengthUnit>(1.235, LengthUnit.FEET)
                .Add(new Quantity<LengthUnit>(0.005, LengthUnit.FEET));

            Assert.AreEqual(1.24, result.Value, EPSILON);
        }

        [TestMethod]
        public void testRounding_Divide_NoRounding()
        {
            double ratio = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Divide(new Quantity<LengthUnit>(3.0, LengthUnit.FEET));

            Assert.AreNotEqual(3.33, ratio, EPSILON); // Raw division, not rounded
            Assert.IsLessThan(0.0001, Math.Abs(ratio - 3.333333333));
        }

        [TestMethod]
        public void testImplicitTargetUnit_AddSubtract()
        {
            var addResult = new Quantity<LengthUnit>(1.0, LengthUnit.FEET)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.INCH));

            var subtractResult = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH));

            Assert.AreEqual(LengthUnit.FEET, addResult.Unit);
            Assert.AreEqual(LengthUnit.FEET, subtractResult.Unit);
        }

        [TestMethod]
        public void testExplicitTargetUnit_AddSubtract_Overrides()
        {
            var addResult = new Quantity<LengthUnit>(1.0, LengthUnit.FEET)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.INCH), LengthUnit.INCH);

            var subtractResult = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH), LengthUnit.INCH);

            Assert.AreEqual(LengthUnit.INCH, addResult.Unit);
            Assert.AreEqual(LengthUnit.INCH, subtractResult.Unit);
        }

        [TestMethod]
        public void testImmutability_AfterAdd_ViaCentralizedHelper()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var second = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var result = first.Add(second);

            Assert.AreEqual(10.0, first.Value);
            Assert.AreEqual(2.0, second.Value);
            Assert.AreEqual(12.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testImmutability_AfterSubtract_ViaCentralizedHelper()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var second = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var result = first.Subtract(second);

            Assert.AreEqual(10.0, first.Value);
            Assert.AreEqual(2.0, second.Value);
            Assert.AreEqual(8.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testImmutability_AfterDivide_ViaCentralizedHelper()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var second = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            double ratio = first.Divide(second);

            Assert.AreEqual(10.0, first.Value);
            Assert.AreEqual(5.0, second.Value);
            Assert.AreEqual(2.0, ratio, EPSILON);
        }

        [TestMethod]
        public void testAllOperations_AcrossAllCategories()
        {
            var length = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.INCH))
                .Subtract(new Quantity<LengthUnit>(2.0, LengthUnit.FEET));

            var weight = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM)
                .Add(new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM))
                .Subtract(new Quantity<WeightUnit>(2000.0, WeightUnit.GRAM));

            var volume = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE)
                .Add(new Quantity<VolumeUnit>(500.0, VolumeUnit.MILLILITRE))
                .Subtract(new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE));

            Assert.AreEqual(9.0, length.Value, EPSILON);
            Assert.AreEqual(13.0, weight.Value, EPSILON);
            Assert.AreEqual(4.5, volume.Value, EPSILON);
        }

        [TestMethod]
        public void testDivideByZero_ConsistentException()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            bool exceptionThrown = false;

            try { first.Divide(new Quantity<LengthUnit>(0.0, LengthUnit.FEET)); }
            catch (ArithmeticException) { exceptionThrown = true; }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void testArithmetic_Chain_Operations()
        {
            var result = new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Add(new Quantity<LengthUnit>(12.0, LengthUnit.INCH))
                .Subtract(new Quantity<LengthUnit>(2.0, LengthUnit.FEET));

            Assert.AreEqual(9.0, result.Value, EPSILON);
        }

        [TestMethod]
        public void testNonCommutative_Subtraction()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);

            var left = a.Subtract(b);
            var right = b.Subtract(a);

            Assert.AreEqual(5.0, left.Value, EPSILON);
            Assert.AreEqual(-5.0, right.Value, EPSILON);
        }

        [TestMethod]
        public void testNonCommutative_Division()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);

            Assert.AreEqual(2.0, a.Divide(b), EPSILON);
            Assert.AreEqual(0.5, b.Divide(a), EPSILON);
        }
    }

    [TestClass]
    public class QuantityUc14Tests_TemperatureAndSelectiveArithmetic
    {
        private const double EPSILON = 1e-6;

        [TestMethod]
        public void testTemperatureEquality_CelsiusToFahrenheit_ZeroPoint()
        {
            var celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var fahrenheit = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);

            Assert.IsTrue(celsius.Equals(fahrenheit));
            Assert.IsTrue(fahrenheit.Equals(celsius));
        }

        [TestMethod]
        public void testTemperatureEquality_CelsiusToKelvin_ZeroPoint()
        {
            var celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var kelvin = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.KELVIN);

            Assert.IsTrue(celsius.Equals(kelvin));
        }

        [TestMethod]
        public void testTemperatureConversion_CelsiusToFahrenheit()
        {
            var converted = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS)
                .ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.AreEqual(TemperatureUnit.FAHRENHEIT, converted.Unit);
            Assert.AreEqual(212.0, converted.Value, EPSILON);
        }

        [TestMethod]
        public void testTemperatureConversion_KelvinToCelsius()
        {
            var converted = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.KELVIN)
                .ConvertTo(TemperatureUnit.CELSIUS);

            Assert.AreEqual(TemperatureUnit.CELSIUS, converted.Unit);
            Assert.AreEqual(0.0, converted.Value, EPSILON);
        }

        [TestMethod]
        public void testTemperatureConversion_Negative40_EqualPoint()
        {
            var converted = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.CELSIUS)
                .ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.AreEqual(-40.0, converted.Value, EPSILON);
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Add_Throws()
        {
            var first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            bool exceptionThrown = false;

            try { _ = first.Add(new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS)); }
            catch (UnsupportedOperationException) { exceptionThrown = true; }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Subtract_Throws()
        {
            var first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            bool exceptionThrown = false;

            try { _ = first.Subtract(new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS)); }
            catch (UnsupportedOperationException) { exceptionThrown = true; }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Divide_Throws()
        {
            var first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            bool exceptionThrown = false;

            try { _ = first.Divide(new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS)); }
            catch (UnsupportedOperationException) { exceptionThrown = true; }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Message_IsClear()
        {
            var first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            string message = string.Empty;

            try
            {
                _ = first.Divide(new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS));
            }
            catch (UnsupportedOperationException ex)
            {
                message = ex.Message;
            }

            Assert.Contains(message, "Temperature does not support divide operation.");
        }
    }
}