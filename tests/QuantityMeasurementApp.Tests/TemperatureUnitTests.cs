using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC14 temperature equality and conversion tests across Celsius, Fahrenheit, and Kelvin.
    /// </summary>
    [TestClass]
    public class TemperatureUnitTests
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Verifies Celsius to Fahrenheit equality for freezing point.
        /// </summary>
        [TestMethod]
        public void Equality_CelsiusAndFahrenheit_FreezingPoint()
        {
            var celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            var fahrenheit = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(celsius.Equals(fahrenheit));
            Assert.IsTrue(fahrenheit.Equals(celsius));
        }

        /// <summary>
        /// Verifies Celsius to Kelvin equality for freezing point.
        /// </summary>
        [TestMethod]
        public void Equality_CelsiusAndKelvin_FreezingPoint()
        {
            var kelvin = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);
            var celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);

            Assert.IsTrue(kelvin.Equals(celsius));
        }

        /// <summary>
        /// Verifies conversion Celsius to Fahrenheit.
        /// </summary>
        [TestMethod]
        public void Convert_CelsiusToFahrenheit_ReturnsExpectedValue()
        {
            var source = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);

            var result = source.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(212.0, result.Value, Epsilon);
            Assert.AreEqual(TemperatureUnit.Fahrenheit, result.Unit);
        }

        /// <summary>
        /// Verifies conversion Fahrenheit to Celsius.
        /// </summary>
        [TestMethod]
        public void Convert_FahrenheitToCelsius_ReturnsExpectedValue()
        {
            var source = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            var result = source.ConvertTo(TemperatureUnit.Celsius);

            Assert.AreEqual(0.0, result.Value, Epsilon);
            Assert.AreEqual(TemperatureUnit.Celsius, result.Unit);
        }

        /// <summary>
        /// Verifies conversion Kelvin to Celsius.
        /// </summary>
        [TestMethod]
        public void Convert_KelvinToCelsius_ReturnsExpectedValue()
        {
            var source = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);

            var result = source.ConvertTo(TemperatureUnit.Celsius);

            Assert.AreEqual(0.0, result.Value, Epsilon);
        }

        /// <summary>
        /// Verifies the equal-point conversion where -40 Celsius equals -40 Fahrenheit.
        /// </summary>
        [TestMethod]
        public void Convert_EqualPoint_NegativeForty()
        {
            var source = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.Celsius);

            var result = source.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(-40.0, result.Value, Epsilon);
        }

        /// <summary>
        /// Verifies absolute-zero equivalence across all temperature units.
        /// </summary>
        [TestMethod]
        public void Equality_AbsoluteZero_AllUnitsEquivalent()
        {
            var celsius = new Quantity<TemperatureUnit>(-273.15, TemperatureUnit.Celsius);
            var kelvin = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Kelvin);
            var fahrenheit = new Quantity<TemperatureUnit>(-459.67, TemperatureUnit.Fahrenheit);

            Assert.IsTrue(celsius.Equals(kelvin));
            Assert.IsTrue(kelvin.Equals(fahrenheit));
            Assert.IsTrue(celsius.Equals(fahrenheit));
        }
    }
}
