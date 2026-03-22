using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC14 tests for unsupported arithmetic semantics on absolute temperatures.
    /// </summary>
    [TestClass]
    public class TemperatureUnsupportedOperationsTests
    {
        /// <summary>
        /// Verifies addition is blocked for absolute temperature quantities.
        /// </summary>
        [TestMethod]
        public void Add_Temperature_ThrowsNotSupportedException()
        {
            var first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var second = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            var exception = Assert.ThrowsException<System.NotSupportedException>(() => first.Add(second));
            StringAssert.Contains(exception.Message, "Temperature does not support");
        }

        /// <summary>
        /// Verifies subtraction is blocked for absolute temperature quantities.
        /// </summary>
        [TestMethod]
        public void Subtract_Temperature_ThrowsNotSupportedException()
        {
            var first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var second = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            var exception = Assert.ThrowsException<System.NotSupportedException>(() =>
                first.Subtract(second)
            );
            StringAssert.Contains(exception.Message, "Temperature does not support");
        }

        /// <summary>
        /// Verifies division is blocked for absolute temperature quantities.
        /// </summary>
        [TestMethod]
        public void Divide_Temperature_ThrowsNotSupportedException()
        {
            var first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var second = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            var exception = Assert.ThrowsException<System.NotSupportedException>(() => first.Divide(second));
            StringAssert.Contains(exception.Message, "Temperature does not support");
        }

        /// <summary>
        /// Verifies operation-support defaults remain true for non-temperature categories.
        /// </summary>
        [TestMethod]
        public void SupportsArithmetic_DefaultTrue_ForLengthWeightVolume()
        {
            Assert.IsTrue(LengthUnit.Feet.AsMeasurable().SupportsArithmetic());
            Assert.IsTrue(WeightUnit.Kilogram.AsMeasurable().SupportsArithmetic());
            Assert.IsTrue(VolumeUnit.Litre.AsMeasurable().SupportsArithmetic());
        }

        /// <summary>
        /// Verifies temperature measurable exposes arithmetic support as false.
        /// </summary>
        [TestMethod]
        public void SupportsArithmetic_False_ForTemperature()
        {
            Assert.IsFalse(TemperatureUnit.Celsius.AsMeasurable().SupportsArithmetic());
        }

        /// <summary>
        /// Verifies cross-category equality remains false.
        /// </summary>
        [TestMethod]
        public void Equals_CrossCategory_TemperatureAndLength_ReturnsFalse()
        {
            object length = new Quantity<LengthUnit>(100.0, LengthUnit.Feet);
            var temperature = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);

            Assert.IsFalse(temperature.Equals(length));
        }
    }
}
