using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Verifies conversion behavior through the service facade for length units.
    /// </summary>
    [TestClass]
    public class UnitConversionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void Convert_FeetToInches_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(1.0, LengthUnit.Feet, LengthUnit.Inches);

            Assert.AreEqual(12.0, result, Epsilon);
        }

        [TestMethod]
        public void Convert_InchesToFeet_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(24.0, LengthUnit.Inches, LengthUnit.Feet);

            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void Convert_CentimetersToInches_ReturnsExpectedValue()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(2.54, LengthUnit.Centimeters, LengthUnit.Inches);

            Assert.AreEqual(1.0, result, Epsilon);
        }

        [TestMethod]
        public void Convert_ZeroValue_ReturnsZero()
        {
            var service = new QuantityMeasurementServiceImpl();

            var result = service.Convert(0.0, LengthUnit.Feet, LengthUnit.Inches);

            Assert.AreEqual(0.0, result, Epsilon);
        }

        [TestMethod]
        public void Convert_RoundTrip_PreservesValueWithinTolerance()
        {
            var service = new QuantityMeasurementServiceImpl();
            const double value = 7.5;

            var converted = service.Convert(value, LengthUnit.Yards, LengthUnit.Centimeters);
            var roundTrip = service.Convert(converted, LengthUnit.Centimeters, LengthUnit.Yards);

            Assert.AreEqual(value, roundTrip, Epsilon);
        }

        [TestMethod]
        public void Convert_NaN_ThrowsArgumentException()
        {
            var service = new QuantityMeasurementServiceImpl();

            Assert.ThrowsException<ArgumentException>(() =>
                service.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inches)
            );
        }
    }
}
