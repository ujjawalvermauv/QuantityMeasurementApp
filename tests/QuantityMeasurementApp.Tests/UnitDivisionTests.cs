using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC12 division tests for length quantities, including ratio semantics and division safeguards.
    /// </summary>
    [TestClass]
    public class UnitDivisionTests
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Verifies division for same-unit operands.
        /// </summary>
        [TestMethod]
        public void Division_SameUnit_ReturnsExpectedRatio()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Divide(10.0, LengthUnit.Feet, 2.0, LengthUnit.Feet);

            Assert.AreEqual(5.0, result, Epsilon);
        }

        /// <summary>
        /// Verifies division for cross-unit operands in the same category.
        /// </summary>
        [TestMethod]
        public void Division_CrossUnit_InchesByFeet_ReturnsOne()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Divide(24.0, LengthUnit.Inches, 2.0, LengthUnit.Feet);

            Assert.AreEqual(1.0, result, Epsilon);
        }

        /// <summary>
        /// Verifies null second operand is rejected.
        /// </summary>
        [TestMethod]
        public void Division_NullSecondOperand_ThrowsArgumentNullException()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            Assert.ThrowsException<System.ArgumentNullException>(() => first.Divide(null!));
        }

        /// <summary>
        /// Verifies type safety blocks cross-category subtraction/division calls.
        /// </summary>
        [TestMethod]
        public void Division_CrossCategoryInvocation_IsRejectedByRuntimeTypeChecks()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            object invalidOperand = new Quantity<WeightUnit>(5.0, WeightUnit.Kilogram);
            var method = typeof(Quantity<LengthUnit>).GetMethod(
                nameof(Quantity<LengthUnit>.Divide)
            );

            Assert.IsNotNull(method);
            Assert.ThrowsException<System.ArgumentException>(() =>
                method!.Invoke(first, new[] { invalidOperand })
            );
        }
    }
}
