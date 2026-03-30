using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    ///<summary>
    /// The LengthUnitTests class verifies that the standalone LengthUnit enum and its
    /// conversion responsibilities work correctly for conversion factors, conversion to base unit,
    /// and conversion from base unit.
    /// </summary>
    /// <remarks>
    /// Confirms enum-extension conversions used by generic UC10 quantity operations.
    /// </remarks>
    [TestClass]
    public class LengthUnitTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        // Verifies that the Feet constant has conversion factor 1.0.
        public void LengthUnitEnum_FeetConstant()
        {
            Assert.AreEqual(1.0, LengthUnit.Feet.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        // Verifies that the Inches constant has conversion factor 1/12.
        public void LengthUnitEnum_InchesConstant()
        {
            Assert.AreEqual(1.0 / 12.0, LengthUnit.Inches.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        // Verifies that the Yards constant has conversion factor 3.0.
        public void LengthUnitEnum_YardsConstant()
        {
            Assert.AreEqual(3.0, LengthUnit.Yards.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        // Verifies that the Centimeters constant has conversion factor 1/30.48.
        public void LengthUnitEnum_CentimetersConstant()
        {
            Assert.AreEqual(1.0 / 30.48, LengthUnit.Centimeters.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        // Verifies base-unit conversion for feet.
        public void ConvertToBaseUnit_FeetToFeet()
        {
            Assert.AreEqual(5.0, LengthUnit.Feet.ConvertToBaseUnit(5.0), Epsilon);
        }

        [TestMethod]
        // Verifies conversion from inches to feet.
        public void ConvertToBaseUnit_InchesToFeet()
        {
            Assert.AreEqual(1.0, LengthUnit.Inches.ConvertToBaseUnit(12.0), Epsilon);
        }

        [TestMethod]
        // Verifies conversion from yards to feet.
        public void ConvertToBaseUnit_YardsToFeet()
        {
            Assert.AreEqual(3.0, LengthUnit.Yards.ConvertToBaseUnit(1.0), Epsilon);
        }

        [TestMethod]
        // Verifies conversion from centimeters to feet.
        public void ConvertToBaseUnit_CentimetersToFeet()
        {
            Assert.AreEqual(1.0, LengthUnit.Centimeters.ConvertToBaseUnit(30.48), Epsilon);
        }

        [TestMethod]
        // Verifies conversion from base feet to feet.
        public void ConvertFromBaseUnit_FeetToFeet()
        {
            Assert.AreEqual(2.0, LengthUnit.Feet.ConvertFromBaseUnit(2.0), Epsilon);
        }

        [TestMethod]
        // Verifies conversion from base feet to inches.
        public void ConvertFromBaseUnit_FeetToInches()
        {
            Assert.AreEqual(12.0, LengthUnit.Inches.ConvertFromBaseUnit(1.0), Epsilon);
        }

        [TestMethod]
        // Verifies conversion from base feet to yards.
        public void ConvertFromBaseUnit_FeetToYards()
        {
            Assert.AreEqual(1.0, LengthUnit.Yards.ConvertFromBaseUnit(3.0), Epsilon);
        }

        [TestMethod]
        // Verifies conversion from base feet to centimeters.
        public void ConvertFromBaseUnit_FeetToCentimeters()
        {
            Assert.AreEqual(30.48, LengthUnit.Centimeters.ConvertFromBaseUnit(1.0), Epsilon);
        }
    }
}
