using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Verifies weight category conversions through generic service methods.
    /// </summary>
    [TestClass]
    public class WeightUnitConversionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void Convert_KilogramToGram_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(1.0, WeightUnit.Kilogram, WeightUnit.Gram);

            Assert.AreEqual(1000.0, result, Epsilon);
        }

        [TestMethod]
        public void Convert_PoundToKilogram_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(2.0, WeightUnit.Pound, WeightUnit.Kilogram);

            Assert.AreEqual(0.91, result, Epsilon);
        }

        [TestMethod]
        public void Convert_GramToPound_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(500.0, WeightUnit.Gram, WeightUnit.Pound);

            Assert.AreEqual(1.10, result, Epsilon);
        }

        [TestMethod]
        public void Convert_RoundTrip_PreservesValueWithinTolerance()
        {
            var service = new QuantityMeasurementServiceImpl();
            const double value = 1.5;

            var grams = service.Convert(value, WeightUnit.Kilogram, WeightUnit.Gram);
            var roundTrip = service.Convert(grams, WeightUnit.Gram, WeightUnit.Kilogram);

            Assert.AreEqual(value, roundTrip, Epsilon);
        }

        [TestMethod]
        public void Convert_InvalidTargetUnit_ThrowsArgumentException()
        {
            var service = new QuantityMeasurementServiceImpl();

            Assert.ThrowsException<ArgumentException>(() =>
                service.Convert(1.0, WeightUnit.Kilogram, (WeightUnit)999)
            );
        }
    }
}
