using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class VolumeUnitTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void VolumeUnitEnum_LitreConstant()
        {
            Assert.AreEqual(1.0, VolumeUnit.Litre.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        public void VolumeUnitEnum_MillilitreConstant()
        {
            Assert.AreEqual(0.001, VolumeUnit.Millilitre.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        public void VolumeUnitEnum_GallonConstant()
        {
            Assert.AreEqual(3.78541, VolumeUnit.Gallon.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        public void ConvertToBaseUnit_MillilitreToLitre()
        {
            Assert.AreEqual(1.0, VolumeUnit.Millilitre.ConvertToBaseUnit(1000.0), Epsilon);
        }

        [TestMethod]
        public void ConvertToBaseUnit_GallonToLitre()
        {
            Assert.AreEqual(3.78541, VolumeUnit.Gallon.ConvertToBaseUnit(1.0), Epsilon);
        }

        [TestMethod]
        public void ConvertFromBaseUnit_LitreToMillilitre()
        {
            Assert.AreEqual(1000.0, VolumeUnit.Millilitre.ConvertFromBaseUnit(1.0), Epsilon);
        }

        [TestMethod]
        public void ConvertFromBaseUnit_LitreToGallon()
        {
            Assert.AreEqual(
                0.26417217685798894,
                VolumeUnit.Gallon.ConvertFromBaseUnit(1.0),
                Epsilon
            );
        }
    }
}
