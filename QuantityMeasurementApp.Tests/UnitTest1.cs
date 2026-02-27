using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthTest
    {
        [TestMethod] // test for equality of two quantities with same unit and value
        public void GivenFeetToFeet_SameValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod] // test for equality of two quantities with same unit and value
        public void GivenInchToInch_SameValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.INCH);
            var q2 = new QuantityLength(1.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod] // test for equality of two quantities with different units but equivalent value
        public void GivenFeetToInch_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod] // test for equality of two quantities with different units but equivalent value
        public void GivenInchToFeet_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(12.0, LengthUnit.INCH);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod] // test for inequality of two quantities with same unit but different values
        public void GivenFeetDifferentValue_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(2.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod] // test for inequality of two quantities with same unit but different values
        public void GivenInchDifferentValue_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.INCH);
            var q2 = new QuantityLength(2.0, LengthUnit.INCH);

            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod] // test for inequality of two quantities with different units and non-equivalent values
        public void GivenNullComparison_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(null));
        }

        [TestMethod] // test for inequality of two quantities with different types
        public void GivenSameReference_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q1));
        }
        [TestMethod]
        public void GivenYardToFeet_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.YARD);
            var q2 = new QuantityLength(3.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenYardToInch_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.YARD);
            var q2 = new QuantityLength(36.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenCentimeterToInch_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.CENTIMETER);
            var q2 = new QuantityLength(0.393701, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenCentimeterToFeet_NonEquivalent_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.CENTIMETER);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(q2));
        }
        
    }
}