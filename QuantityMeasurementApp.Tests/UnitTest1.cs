using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementAppTest
    {
        // -------- FEET TESTS --------

        [TestMethod]
        public void GivenSameFeetValue_WhenCompared_ShouldReturnTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementApp.CompareFeet(1.0, 1.0));
        }

        [TestMethod]
        public void GivenDifferentFeetValue_WhenCompared_ShouldReturnFalse()
        {
            Assert.IsFalse(
                QuantityMeasurementApp.CompareFeet(1.0, 2.0));
        }

        // -------- INCH TESTS --------

        [TestMethod]
        public void GivenSameInchValue_WhenCompared_ShouldReturnTrue()
        {
            Assert.IsTrue(
                QuantityMeasurementApp.CompareInch(1.0, 1.0));
        }

        [TestMethod]
        public void GivenDifferentInchValue_WhenCompared_ShouldReturnFalse()
        {
            Assert.IsFalse(
                QuantityMeasurementApp.CompareInch(1.0, 2.0));
        }

        [TestMethod]
        public void GivenInchValue_WhenComparedWithNull_ShouldReturnFalse()
        {
            var inch = new QuantityMeasurementApp.Inch(1.0);

            Assert.IsFalse(inch.Equals(null));
        }

        [TestMethod]
        public void GivenSameReference_WhenCompared_ShouldReturnTrue()
        {
            var inch = new QuantityMeasurementApp.Inch(1.0);

            Assert.IsTrue(inch.Equals(inch));
        }
    }
}