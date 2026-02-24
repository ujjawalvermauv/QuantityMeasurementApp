using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementAppTest
    {
        [TestMethod]
        public void GivenSameFeetValue_WhenCompared_ShouldReturnTrue()
        {
            var feet1 = new QuantityMeasurementApp.Feet(1.0);
            var feet2 = new QuantityMeasurementApp.Feet(1.0);

            Assert.IsTrue(feet1.Equals(feet2));
        }

        [TestMethod]
        public void GivenDifferentFeetValue_WhenCompared_ShouldReturnFalse()
        {
            var feet1 = new QuantityMeasurementApp.Feet(1.0);
            var feet2 = new QuantityMeasurementApp.Feet(2.0);

            Assert.IsFalse(feet1.Equals(feet2));
        }

        [TestMethod]
        public void GivenFeetValue_WhenComparedWithNull_ShouldReturnFalse()
        {
            var feet1 = new QuantityMeasurementApp.Feet(1.0);

            Assert.IsFalse(feet1.Equals(null));
        }

        [TestMethod]
        public void GivenSameReference_WhenCompared_ShouldReturnTrue()
        {
            var feet1 = new QuantityMeasurementApp.Feet(1.0);

            Assert.IsTrue(feet1.Equals(feet1));
        }
    }
}