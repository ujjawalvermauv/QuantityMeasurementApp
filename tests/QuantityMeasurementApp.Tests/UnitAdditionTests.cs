using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Tests
{
    ///<summary>
    /// The UnitAdditionTests class contains comprehensive unit tests for the addition functionality of the QuantityMeasurement
    /// service, covering both same-unit and cross-unit addition scenarios. The tests verify that the service correctly handles:
    /// - Same-unit addition (e.g., feet + feet, inches + inches)
    /// - Cross-unit addition with various target units (e.g., feet + inches, yards + feet, centimeters + inches)
    /// </summary>
    /// <remarks>
    /// Includes rounding-aware assertions aligned with UC10 two-decimal result policy.
    /// </remarks>
    [TestClass]
    // UC6 test coverage for same-unit and cross-unit addition behavior.
    public class UnitAdditionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        // Verifies same-unit addition in feet (no conversion required).
        public void Addition_SameUnit_FeetPlusFeet()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(1.0, LengthUnit.Feet, 2.0, LengthUnit.Feet, LengthUnit.Feet);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        // Verifies same-unit addition in inches.
        public void Addition_SameUnit_InchesPlusInches()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                6.0,
                LengthUnit.Inches,
                6.0,
                LengthUnit.Inches,
                LengthUnit.Inches
            );

            Assert.AreEqual(12.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inches, result.Unit);
        }

        [TestMethod]
        // Verifies cross-unit addition with result requested in feet.
        public void Addition_CrossUnit_FeetPlusInches_ResultInFeet()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                1.0,
                LengthUnit.Feet,
                12.0,
                LengthUnit.Inches,
                LengthUnit.Feet
            );

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        // Verifies cross-unit addition with result requested in inches.
        public void Addition_CrossUnit_InchesPlusFeet_ResultInInches()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                12.0,
                LengthUnit.Inches,
                1.0,
                LengthUnit.Feet,
                LengthUnit.Inches
            );

            Assert.AreEqual(24.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inches, result.Unit);
        }

        [TestMethod]
        // Verifies yard and feet addition with result in yards.
        public void Addition_CrossUnit_YardsPlusFeet_ResultInYards()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(1.0, LengthUnit.Yards, 3.0, LengthUnit.Feet, LengthUnit.Yards);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Yards, result.Unit);
        }

        [TestMethod]
        // Verifies centimeter and inch addition with floating-point tolerance.
        public void Addition_CrossUnit_CentimetersPlusInches_ResultInCentimeters()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                2.54,
                LengthUnit.Centimeters,
                1.0,
                LengthUnit.Inches,
                LengthUnit.Centimeters
            );

            Assert.AreEqual(5.08, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Centimeters, result.Unit);
        }

        [TestMethod]
        // Verifies commutativity when both results are compared in the same target unit.
        public void Addition_Commutative_WhenComparedInSameTargetUnit()
        {
            var service = new QuantityMeasurementServiceImpl();

            var first = service.Add(1.0, LengthUnit.Feet, 12.0, LengthUnit.Inches, LengthUnit.Feet);
            var second = service.Add(
                12.0,
                LengthUnit.Inches,
                1.0,
                LengthUnit.Feet,
                LengthUnit.Feet
            );

            Assert.AreEqual(first.Value, second.Value, Epsilon);
        }

        [TestMethod]
        // Verifies additive identity: adding zero does not change the value.
        public void Addition_WithZero_ReturnsSameValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(5.0, LengthUnit.Feet, 0.0, LengthUnit.Inches, LengthUnit.Feet);

            Assert.AreEqual(5.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        // Verifies precision behavior for mixed large and very small magnitudes.
        public void Addition_LargeAndSmallValues_MaintainsPrecision()
        {
            var service = new QuantityMeasurementServiceImpl();
            var large = 1_000_000_000.0;
            var small = 0.0001;

            var result = service.Add(
                large,
                LengthUnit.Feet,
                small,
                LengthUnit.Feet,
                LengthUnit.Feet
            );

            Assert.AreEqual(large, result.Value, Epsilon);
        }

        [TestMethod]
        // Verifies invalid target units are rejected.
        public void Addition_InvalidTargetUnit_ThrowsArgumentException()
        {
            var service = new QuantityMeasurementServiceImpl();
            var invalidUnit = (LengthUnit)999;

            Assert.ThrowsException<ArgumentException>(() =>
                service.Add(1.0, LengthUnit.Feet, 1.0, LengthUnit.Inches, invalidUnit)
            );
        }

        [TestMethod]
        // Verifies null operand handling in service overload.
        public void Addition_NullMeasurement_ThrowsArgumentNullException()
        {
            var service = new QuantityMeasurementServiceImpl();
            Quantity<LengthUnit> second = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.ThrowsException<ArgumentNullException>(() =>
                service.Add(null!, second, LengthUnit.Feet)
            );
        }

        [TestMethod]
        // Verifies explicit target as a third unit (different from both operands).
        public void Addition_ExplicitTargetUnit_Yards_FromFeetAndInches()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                1.0,
                LengthUnit.Feet,
                12.0,
                LengthUnit.Inches,
                LengthUnit.Yards
            );

            Assert.AreEqual(0.67, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Yards, result.Unit);
        }

        [TestMethod]
        // Verifies explicit target unit conversion to centimeters.
        public void Addition_ExplicitTargetUnit_Centimeters_FromInches()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                1.0,
                LengthUnit.Inches,
                1.0,
                LengthUnit.Inches,
                LengthUnit.Centimeters
            );

            Assert.AreEqual(5.08, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Centimeters, result.Unit);
        }

        [TestMethod]
        // Verifies explicit target matching first operand unit.
        public void Addition_ExplicitTargetUnit_SameAsFirstOperand()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(2.0, LengthUnit.Yards, 3.0, LengthUnit.Feet, LengthUnit.Yards);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Yards, result.Unit);
        }

        [TestMethod]
        // Verifies explicit target matching second operand unit.
        public void Addition_ExplicitTargetUnit_SameAsSecondOperand()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(2.0, LengthUnit.Yards, 3.0, LengthUnit.Feet, LengthUnit.Feet);

            Assert.AreEqual(9.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        // Verifies commutativity with explicit third target unit.
        public void Addition_ExplicitTargetUnit_CommutativeInYards()
        {
            var service = new QuantityMeasurementServiceImpl();

            var first = service.Add(
                1.0,
                LengthUnit.Feet,
                12.0,
                LengthUnit.Inches,
                LengthUnit.Yards
            );
            var second = service.Add(
                12.0,
                LengthUnit.Inches,
                1.0,
                LengthUnit.Feet,
                LengthUnit.Yards
            );

            Assert.AreEqual(first.Value, second.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Yards, first.Unit);
            Assert.AreEqual(LengthUnit.Yards, second.Unit);
        }

        [TestMethod]
        // Verifies zero operand with explicit target conversion.
        public void Addition_ExplicitTargetUnit_WithZero_ResultInYards()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                5.0,
                LengthUnit.Feet,
                0.0,
                LengthUnit.Inches,
                LengthUnit.Yards
            );

            Assert.AreEqual(1.67, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Yards, result.Unit);
        }

        [TestMethod]
        // Verifies negative value addition with explicit target conversion.
        public void Addition_ExplicitTargetUnit_WithNegative_ResultInInches()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Add(
                5.0,
                LengthUnit.Feet,
                -2.0,
                LengthUnit.Feet,
                LengthUnit.Inches
            );

            Assert.AreEqual(36.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inches, result.Unit);
        }

        [TestMethod]
        // Verifies mathematical equivalence across different target units.
        public void Addition_ExplicitTargetUnit_MathematicalEquivalenceAcrossTargets()
        {
            var service = new QuantityMeasurementServiceImpl();

            var inFeet = service.Add(
                1.0,
                LengthUnit.Feet,
                12.0,
                LengthUnit.Inches,
                LengthUnit.Feet
            );
            var inInches = service.Add(
                1.0,
                LengthUnit.Feet,
                12.0,
                LengthUnit.Inches,
                LengthUnit.Inches
            );

            Assert.AreEqual(2.0, inFeet.Value, Epsilon);
            Assert.AreEqual(24.0, inInches.Value, Epsilon);
            Assert.AreEqual(inFeet.Value * 12.0, inInches.Value, Epsilon);
        }
    }
}
