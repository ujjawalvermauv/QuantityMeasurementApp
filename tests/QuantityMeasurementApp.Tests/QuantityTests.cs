using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Equality-focused tests for generic <see cref="Quantity{U}"/> with <see cref="LengthUnit"/>.
    /// Covers equivalent cross-unit values, non-equivalent values, null checks, and reflexive behavior.
    /// </summary>
    /// <remarks>
    /// These tests confirm category-specific correctness while sharing a single generic quantity implementation.
    /// </remarks>
    [TestClass]
    public class QuantityTests
    {
        [TestMethod]
        public void TestEquality_FeetToFeet_SameValue()
        {
            var first = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var second = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void TestEquality_InchToInch_SameValue()
        {
            var first = new Quantity<LengthUnit>(1.0, LengthUnit.Inches);
            var second = new Quantity<LengthUnit>(1.0, LengthUnit.Inches);

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void TestEquality_InchToFeet_EquivalentValue()
        {
            var inches = new Quantity<LengthUnit>(12.0, LengthUnit.Inches);
            var feet = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.IsTrue(inches.Equals(feet));
        }

        [TestMethod]
        public void TestEquality_FeetToFeet_DifferentValue()
        {
            var first = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var second = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void TestEquality_YardSameReference()
        {
            var yard = new Quantity<LengthUnit>(1.0, LengthUnit.Yards);

            Assert.IsTrue(yard.Equals(yard));
        }

        [TestMethod]
        public void TestEquality_CentimeterToFeet_DifferentValue()
        {
            var centimeters = new Quantity<LengthUnit>(1.0, LengthUnit.Centimeters);
            var feet = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.IsFalse(centimeters.Equals(feet));
        }

        [TestMethod]
        public void TestEquality_CentimeterNullComparison()
        {
            var centimeter = new Quantity<LengthUnit>(1.0, LengthUnit.Centimeters);

            Assert.IsFalse(centimeter.Equals(null));
        }
    }
}
