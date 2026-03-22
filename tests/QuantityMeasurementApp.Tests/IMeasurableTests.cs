using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Verifies that unit adapters satisfy the <see cref="IMeasurable"/> contract
    /// for conversion factor, base-unit transforms, and canonical unit names.
    /// </summary>
    /// <remarks>
    /// Protects the abstraction seam that allows enums to participate in UC10 generic behavior.
    /// </remarks>
    [TestClass]
    public class IMeasurableTests
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Length adapter contract check using feet/inches.
        /// </summary>
        [TestMethod]
        public void LengthUnit_AsMeasurable_ImplementsContract()
        {
            IMeasurable measurable = LengthUnit.Feet.AsMeasurable();

            Assert.AreEqual(1.0, measurable.GetConversionFactor(), Epsilon);
            Assert.AreEqual(2.0, measurable.ConvertToBaseUnit(2.0), Epsilon);
            Assert.AreEqual(
                24.0,
                LengthUnit.Inches.AsMeasurable().ConvertFromBaseUnit(2.0),
                Epsilon
            );
            Assert.AreEqual("FEET", measurable.GetUnitName());
        }

        /// <summary>
        /// Weight adapter contract check using grams.
        /// </summary>
        [TestMethod]
        public void WeightUnit_AsMeasurable_ImplementsContract()
        {
            IMeasurable measurable = WeightUnit.Gram.AsMeasurable();

            Assert.AreEqual(0.001, measurable.GetConversionFactor(), Epsilon);
            Assert.AreEqual(1.0, measurable.ConvertToBaseUnit(1000.0), Epsilon);
            Assert.AreEqual(1000.0, measurable.ConvertFromBaseUnit(1.0), Epsilon);
            Assert.AreEqual("GRAM", measurable.GetUnitName());
        }
    }
}
