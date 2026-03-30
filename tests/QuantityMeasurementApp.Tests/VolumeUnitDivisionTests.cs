using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC12 division tests for volume quantities.
    /// </summary>
    [TestClass]
    public class VolumeUnitDivisionTests
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Verifies same-unit division in volume category.
        /// </summary>
        [TestMethod]
        public void Division_LitreByLitre_ReturnsExpectedRatio()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Divide(5.0, VolumeUnit.Litre, 10.0, VolumeUnit.Litre);

            Assert.AreEqual(0.5, result, Epsilon);
        }

        /// <summary>
        /// Verifies cross-unit division in volume category.
        /// </summary>
        [TestMethod]
        public void Division_MillilitreByLitre_ReturnsExpectedRatio()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Divide(1000.0, VolumeUnit.Millilitre, 1.0, VolumeUnit.Litre);

            Assert.AreEqual(1.0, result, Epsilon);
        }
    }
}
