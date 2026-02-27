using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthEqualityTest
    {
        // ---------------- UC1 & UC2 ----------------

        [TestMethod]
        public void GivenFeetToFeet_SameValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenFeetDifferentValue_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(2.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(q2));
        }

        // ---------------- UC2 ----------------

        [TestMethod]
        public void GivenInchToInch_SameValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.INCH);
            var q2 = new QuantityLength(1.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenInchDifferentValue_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.INCH);
            var q2 = new QuantityLength(2.0, LengthUnit.INCH);

            Assert.IsFalse(q1.Equals(q2));
        }

        // ---------------- UC3 Cross Unit ----------------

        [TestMethod]
        public void GivenFeetToInch_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void GivenInchToFeet_EquivalentValue_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(12.0, LengthUnit.INCH);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        // ---------------- UC4 Yard Support ----------------

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

        // ---------------- UC4 Centimeter Support ----------------

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

        // ---------------- Object Behavior ----------------

        [TestMethod]
        public void GivenNullComparison_ShouldReturnFalse()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsFalse(q1.Equals(null));
        }

        [TestMethod]
        public void GivenSameReference_ShouldReturnTrue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsTrue(q1.Equals(q1));
        }
    }
}