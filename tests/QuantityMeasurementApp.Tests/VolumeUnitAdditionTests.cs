using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class VolumeUnitAdditionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void Addition_SameUnit_LitrePlusLitre()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(1.0, VolumeUnit.Litre, 2.0, VolumeUnit.Litre);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, result.Unit);
        }

        [TestMethod]
        public void Addition_CrossUnit_LitrePlusMillilitre_DefaultFirstOperandUnit()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(1.0, VolumeUnit.Litre, 1000.0, VolumeUnit.Millilitre);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, result.Unit);
        }

        [TestMethod]
        public void Addition_CrossUnit_GallonPlusLitre_DefaultFirstOperandUnit()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(2.0, VolumeUnit.Gallon, 3.78541, VolumeUnit.Litre);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Gallon, result.Unit);
        }
    }
}
