using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC13 regression tests that verify centralized arithmetic refactoring preserves UC12 behavior.
    /// </summary>
    [TestClass]
    public class QuantityUc13RefactorTests
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Verifies add, subtract, and divide preserve expected numerical behavior after refactor.
        /// </summary>
        [TestMethod]
        public void Arithmetic_BehaviorPreserved_AfterCentralizedRefactor()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var second = new Quantity<LengthUnit>(6.0, LengthUnit.Inches);

            var added = first.Add(second);
            var subtracted = first.Subtract(second);
            var divided = first.Divide(new Quantity<LengthUnit>(2.0, LengthUnit.Feet));

            Assert.AreEqual(10.5, added.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, added.Unit);
            Assert.AreEqual(9.5, subtracted.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, subtracted.Unit);
            Assert.AreEqual(5.0, divided, Epsilon);
        }

        /// <summary>
        /// Verifies null operand handling is consistent across all arithmetic operations.
        /// </summary>
        [TestMethod]
        public void Arithmetic_NullOperand_ConsistentAcrossOperations()
        {
            var quantity = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            Assert.ThrowsException<System.ArgumentNullException>(() => quantity.Add(null!));
            Assert.ThrowsException<System.ArgumentNullException>(() => quantity.Subtract(null!));
            Assert.ThrowsException<System.ArgumentNullException>(() => quantity.Divide(null!));
        }

        /// <summary>
        /// Verifies divide-by-zero remains fail-fast with arithmetic exception.
        /// </summary>
        [TestMethod]
        public void Divide_ZeroDivisor_ThrowsArithmeticException()
        {
            var quantity = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var zero = new Quantity<LengthUnit>(0.0, LengthUnit.Feet);

            Assert.ThrowsException<System.ArithmeticException>(() => quantity.Divide(zero));
        }
    }
}
