using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC12 subtraction tests for volume quantities.
    /// </summary>
    [TestClass]
    public class VolumeUnitSubtractionTests
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Verifies cross-unit subtraction in default first operand unit.
        /// </summary>
        [TestMethod]
        public void Subtraction_LitreMinusMillilitre_DefaultTargetLitre()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Subtract(5.0, VolumeUnit.Litre, 500.0, VolumeUnit.Millilitre);

            Assert.AreEqual(4.5, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, result.Unit);
        }

        /// <summary>
        /// Verifies explicit target subtraction in millilitres.
        /// </summary>
        [TestMethod]
        public void Subtraction_LitreMinusLitre_ExplicitTargetMillilitre()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Subtract(
                5.0,
                VolumeUnit.Litre,
                2.0,
                VolumeUnit.Litre,
                VolumeUnit.Millilitre
            );

            Assert.AreEqual(3000.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Millilitre, result.Unit);
        }
    }
}
