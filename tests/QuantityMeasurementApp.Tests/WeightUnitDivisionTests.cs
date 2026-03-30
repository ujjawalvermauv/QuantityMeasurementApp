using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC12 division tests for weight quantities.
    /// </summary>
    [TestClass]
    public class WeightUnitDivisionTests
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Verifies same-unit division in weight category.
        /// </summary>
        [TestMethod]
        public void Division_KilogramByKilogram_ReturnsExpectedRatio()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Divide(10.0, WeightUnit.Kilogram, 5.0, WeightUnit.Kilogram);

            Assert.AreEqual(2.0, result, Epsilon);
        }

        /// <summary>
        /// Verifies cross-unit division in weight category.
        /// </summary>
        [TestMethod]
        public void Division_GramByKilogram_ReturnsExpectedRatio()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Divide(2000.0, WeightUnit.Gram, 1.0, WeightUnit.Kilogram);

            Assert.AreEqual(2.0, result, Epsilon);
        }
    }
}
