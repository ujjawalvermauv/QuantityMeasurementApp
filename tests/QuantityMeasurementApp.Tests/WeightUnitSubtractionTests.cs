using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC12 subtraction tests for weight quantities.
    /// </summary>
    [TestClass]
    public class WeightUnitSubtractionTests
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Verifies cross-unit subtraction in default first operand unit.
        /// </summary>
        [TestMethod]
        public void Subtraction_KilogramMinusGram_DefaultTargetKilogram()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Subtract(10.0, WeightUnit.Kilogram, 5000.0, WeightUnit.Gram);

            Assert.AreEqual(5.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        /// <summary>
        /// Verifies explicit target subtraction in grams.
        /// </summary>
        [TestMethod]
        public void Subtraction_KilogramMinusGram_ExplicitTargetGram()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Subtract(
                10.0,
                WeightUnit.Kilogram,
                5000.0,
                WeightUnit.Gram,
                WeightUnit.Gram
            );

            Assert.AreEqual(5000.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }
    }
}
