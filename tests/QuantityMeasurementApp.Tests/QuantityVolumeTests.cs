using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityVolumeTests
    {
        [TestMethod]
        public void TestEquality_LitreToLitre_SameValue()
        {
            var first = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var second = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void TestEquality_LitreToLitre_DifferentValue()
        {
            var first = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var second = new Quantity<VolumeUnit>(2.0, VolumeUnit.Litre);

            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void TestEquality_LitreToMillilitre_EquivalentValue()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var millilitre = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            Assert.IsTrue(litre.Equals(millilitre));
            Assert.IsTrue(millilitre.Equals(litre));
        }

        [TestMethod]
        public void TestEquality_LitreToGallon_EquivalentValue()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var gallonEquivalent = new Quantity<VolumeUnit>(0.26417217685798894, VolumeUnit.Gallon);

            Assert.IsTrue(litre.Equals(gallonEquivalent));
            Assert.IsTrue(gallonEquivalent.Equals(litre));
        }
    }
}
