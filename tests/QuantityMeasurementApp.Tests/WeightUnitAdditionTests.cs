using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Verifies same-unit and cross-unit addition behavior for weight quantities.
    /// </summary>
    [TestClass]
    public class WeightUnitAdditionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void Addition_SameUnit_KilogramPlusKilogram()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(1.0, WeightUnit.Kilogram, 2.0, WeightUnit.Kilogram);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void Addition_CrossUnit_KilogramPlusGram_DefaultFirstOperandUnit()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(1.0, WeightUnit.Kilogram, 1000.0, WeightUnit.Gram);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void Addition_CrossUnit_PoundPlusKilogram_DefaultFirstOperandUnit()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                2.2046244201837775,
                WeightUnit.Pound,
                1.0,
                WeightUnit.Kilogram
            );

            Assert.AreEqual(4.41, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Pound, result.Unit);
        }

        [TestMethod]
        public void Addition_ExplicitTargetUnit_Gram()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                1.0,
                WeightUnit.Kilogram,
                1000.0,
                WeightUnit.Gram,
                WeightUnit.Gram
            );

            Assert.AreEqual(2000.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [TestMethod]
        public void Addition_ExplicitTargetUnit_Pound()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                1.0,
                WeightUnit.Pound,
                453.592,
                WeightUnit.Gram,
                WeightUnit.Pound
            );

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Pound, result.Unit);
        }

        [TestMethod]
        public void Addition_Commutative_WithExplicitTargetUnit()
        {
            var service = new QuantityMeasurementServiceImpl();

            var first = service.Add(
                1.0,
                WeightUnit.Kilogram,
                1000.0,
                WeightUnit.Gram,
                WeightUnit.Kilogram
            );
            var second = service.Add(
                1000.0,
                WeightUnit.Gram,
                1.0,
                WeightUnit.Kilogram,
                WeightUnit.Kilogram
            );

            Assert.AreEqual(first.Value, second.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, first.Unit);
        }

        [TestMethod]
        public void Addition_WithZero_ReturnsSameValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(5.0, WeightUnit.Kilogram, 0.0, WeightUnit.Gram);

            Assert.AreEqual(5.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void Addition_NegativeValues_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(5.0, WeightUnit.Kilogram, -2000.0, WeightUnit.Gram);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void Addition_InvalidTargetUnit_ThrowsArgumentException()
        {
            var service = new QuantityMeasurementServiceImpl();

            Assert.ThrowsException<ArgumentException>(() =>
                service.Add(1.0, WeightUnit.Kilogram, 1.0, WeightUnit.Gram, (WeightUnit)999)
            );
        }
    }
}
