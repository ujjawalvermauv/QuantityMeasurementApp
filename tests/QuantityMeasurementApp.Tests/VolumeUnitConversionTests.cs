using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class VolumeUnitConversionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void Convert_LitreToMillilitre_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(1.0, VolumeUnit.Litre, VolumeUnit.Millilitre);

            Assert.AreEqual(1000.0, result, Epsilon);
        }

        [TestMethod]
        public void Convert_GallonToLitre_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(2.0, VolumeUnit.Gallon, VolumeUnit.Litre);

            Assert.AreEqual(7.57, result, Epsilon);
        }

        [TestMethod]
        public void Convert_MillilitreToGallon_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(500.0, VolumeUnit.Millilitre, VolumeUnit.Gallon);

            Assert.AreEqual(0.13, result, Epsilon);
        }

        [TestMethod]
        public void Convert_ZeroValue_ReturnsZero()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(0.0, VolumeUnit.Litre, VolumeUnit.Millilitre);

            Assert.AreEqual(0.0, result, Epsilon);
        }
    }
}
