using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC12 subtraction tests for length quantities, including implicit/explicit target unit behavior and edge cases.
    /// </summary>
    [TestClass]
    public class UnitSubtractionTests
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Verifies subtraction when both operands use the same unit.
        /// </summary>
        [TestMethod]
        public void Subtraction_SameUnit_FeetMinusFeet()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Subtract(10.0, LengthUnit.Feet, 6.0, LengthUnit.Feet);

            Assert.AreEqual(4.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        /// <summary>
        /// Verifies subtraction with different units and implicit first-operand target unit.
        /// </summary>
        [TestMethod]
        public void Subtraction_CrossUnit_FeetMinusInches_ImplicitTarget()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Subtract(10.0, LengthUnit.Feet, 6.0, LengthUnit.Inches);

            Assert.AreEqual(9.5, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        /// <summary>
        /// Verifies subtraction with different units and explicit target unit.
        /// </summary>
        [TestMethod]
        public void Subtraction_CrossUnit_FeetMinusInches_ExplicitTargetInches()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Subtract(
                10.0,
                LengthUnit.Feet,
                6.0,
                LengthUnit.Inches,
                LengthUnit.Inches
            );

            Assert.AreEqual(114.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inches, result.Unit);
        }

        /// <summary>
        /// Verifies subtraction can produce negative values.
        /// </summary>
        [TestMethod]
        public void Subtraction_WhenSecondIsLarger_ReturnsNegativeValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Subtract(5.0, LengthUnit.Feet, 10.0, LengthUnit.Feet);

            Assert.AreEqual(-5.0, result.Value, Epsilon);
        }
    }
}
