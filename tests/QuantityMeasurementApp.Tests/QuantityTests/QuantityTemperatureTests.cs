using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.QuantityTests
{
    /// <summary>
    /// Tests for temperature-based quantity operations with selective arithmetic support (UC14).
    /// Tests equality, conversion, and rejection of arithmetic operations.
    /// </summary>
    [TestFixture]
    public class QuantityTemperatureTests
    {
        private const double EPSILON = 1e-6;

        /// <summary>
        /// Tests equality between equivalent temperature quantities across all units.
        /// 0°C = 32°F = 273.15K (absolute zero reference point)
        /// </summary>
        [Test]
        public void testTemperature_Equality_AbsoluteZero()
        {
            var celsius = new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS);
            var fahrenheit = new Quantity<TemperatureUnit>(32, TemperatureUnit.FAHRENHEIT);
            var kelvin = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.KELVIN);

            Assert.That(celsius.Equals(fahrenheit), Is.True);
            Assert.That(celsius.Equals(kelvin), Is.True);
            Assert.That(fahrenheit.Equals(kelvin), Is.True);
        }

        /// <summary>
        /// Tests equality between equivalent temperature quantities at freezing point.
        /// 0°C = 32°F = 273.15K
        /// </summary>
        [Test]
        public void testTemperature_Equality_FreezingPoint()
        {
            var celsius = new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS);
            var fahrenheit = new Quantity<TemperatureUnit>(32, TemperatureUnit.FAHRENHEIT);
            var kelvin = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.KELVIN);

            Assert.That(celsius.Equals(fahrenheit), Is.True);
            Assert.That(celsius.Equals(kelvin), Is.True);
            Assert.That(fahrenheit.Equals(kelvin), Is.True);
        }

        /// <summary>
        /// Tests equality between equivalent temperature quantities at boiling point.
        /// 100°C = 212°F = 373.15K
        /// </summary>
        [Test]
        public void testTemperature_Equality_BoilingPoint()
        {
            var celsius = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);
            var fahrenheit = new Quantity<TemperatureUnit>(212, TemperatureUnit.FAHRENHEIT);
            var kelvin = new Quantity<TemperatureUnit>(373.15, TemperatureUnit.KELVIN);

            Assert.That(celsius.Equals(fahrenheit), Is.True);
            Assert.That(celsius.Equals(kelvin), Is.True);
            Assert.That(fahrenheit.Equals(kelvin), Is.True);
        }

        /// <summary>
        /// Tests temperature conversion from Celsius to Fahrenheit.
        /// Formula: °F = (°C × 9/5) + 32
        /// </summary>
        [Test]
        public void testTemperature_Conversion_CelsiusToFahrenheit()
        {
            var celsius = new Quantity<TemperatureUnit>(25, TemperatureUnit.CELSIUS);
            var result = celsius.ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.That(result.Value, Is.EqualTo(77).Within(EPSILON));
        }

        /// <summary>
        /// Tests temperature conversion from Celsius to Kelvin.
        /// Formula: K = °C + 273.15
        /// </summary>
        [Test]
        public void testTemperature_Conversion_CelsiusToKelvin()
        {
            var celsius = new Quantity<TemperatureUnit>(25, TemperatureUnit.CELSIUS);
            var result = celsius.ConvertTo(TemperatureUnit.KELVIN);

            Assert.That(result.Value, Is.EqualTo(298.15).Within(EPSILON));
        }

        /// <summary>
        /// Tests temperature conversion from Fahrenheit to Celsius.
        /// Formula: °C = (°F - 32) × 5/9
        /// </summary>
        [Test]
        public void testTemperature_Conversion_FahrenheitToCelsius()
        {
            var fahrenheit = new Quantity<TemperatureUnit>(77, TemperatureUnit.FAHRENHEIT);
            var result = fahrenheit.ConvertTo(TemperatureUnit.CELSIUS);

            Assert.That(result.Value, Is.EqualTo(25).Within(EPSILON));
        }

        /// <summary>
        /// Tests temperature conversion from Fahrenheit to Kelvin.
        /// Formula: K = (°F - 32) × 5/9 + 273.15
        /// </summary>
        [Test]
        public void testTemperature_Conversion_FahrenheitToKelvin()
        {
            var fahrenheit = new Quantity<TemperatureUnit>(77, TemperatureUnit.FAHRENHEIT);
            var result = fahrenheit.ConvertTo(TemperatureUnit.KELVIN);

            Assert.That(result.Value, Is.EqualTo(298.15).Within(EPSILON));
        }

        /// <summary>
        /// Tests temperature conversion from Kelvin to Celsius.
        /// Formula: °C = K - 273.15
        /// </summary>
        [Test]
        public void testTemperature_Conversion_KelvinToCelsius()
        {
            var kelvin = new Quantity<TemperatureUnit>(298.15, TemperatureUnit.KELVIN);
            var result = kelvin.ConvertTo(TemperatureUnit.CELSIUS);

            Assert.That(result.Value, Is.EqualTo(25).Within(EPSILON));
        }

        /// <summary>
        /// Tests temperature conversion from Kelvin to Fahrenheit.
        /// Formula: °F = (K - 273.15) × 9/5 + 32
        /// </summary>
        [Test]
        public void testTemperature_Conversion_KelvinToFahrenheit()
        {
            var kelvin = new Quantity<TemperatureUnit>(298.15, TemperatureUnit.KELVIN);
            var result = kelvin.ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.That(result.Value, Is.EqualTo(77).Within(EPSILON));
        }

        /// <summary>
        /// Tests that temperature addition is not supported.
        /// Should throw NotSupportedException with appropriate message.
        /// </summary>
        [Test]
        public void testTemperature_Arithmetic_Addition_NotSupported()
        {
            var temp1 = new Quantity<TemperatureUnit>(20, TemperatureUnit.CELSIUS);
            var temp2 = new Quantity<TemperatureUnit>(68, TemperatureUnit.FAHRENHEIT);

            var ex = Assert.Throws<NotSupportedException>(() => temp1.Add(temp2));
            Assert.That(ex.Message, Does.Contain("Temperature does not support add operation"));
        }

        /// <summary>
        /// Tests that temperature subtraction is not supported.
        /// Should throw NotSupportedException with appropriate message.
        /// </summary>
        [Test]
        public void testTemperature_Arithmetic_Subtraction_NotSupported()
        {
            var temp1 = new Quantity<TemperatureUnit>(20, TemperatureUnit.CELSIUS);
            var temp2 = new Quantity<TemperatureUnit>(68, TemperatureUnit.FAHRENHEIT);

            var ex = Assert.Throws<NotSupportedException>(() => temp1.Subtract(temp2));
            Assert.That(ex.Message, Does.Contain("Temperature does not support subtract operation"));
        }

        /// <summary>
        /// Tests that temperature division is not supported.
        /// Should throw NotSupportedException with appropriate message.
        /// </summary>
        [Test]
        public void testTemperature_Arithmetic_Division_NotSupported()
        {
            var temp1 = new Quantity<TemperatureUnit>(20, TemperatureUnit.CELSIUS);
            var temp2 = new Quantity<TemperatureUnit>(68, TemperatureUnit.FAHRENHEIT);

            var ex = Assert.Throws<NotSupportedException>(() => temp1.Divide(temp2));
            Assert.That(ex.Message, Does.Contain("Temperature does not support divide operation"));
        }

        /// <summary>
        /// Tests that temperature quantities are not equal to quantities from other categories.
        /// Cross-category prevention should work for temperature as well.
        /// </summary>
        [Test]
        public void testTemperature_CrossCategoryPrevention()
        {
            var temperature = new Quantity<TemperatureUnit>(20, TemperatureUnit.CELSIUS);
            var length = new Quantity<LengthUnit>(20, LengthUnit.FEET);

            Assert.That(temperature.Equals(length), Is.False);
        }

        /// <summary>
        /// Tests that temperature quantities with different values are not equal.
        /// </summary>
        [Test]
        public void testTemperature_Inequality()
        {
            var temp1 = new Quantity<TemperatureUnit>(20, TemperatureUnit.CELSIUS);
            var temp2 = new Quantity<TemperatureUnit>(25, TemperatureUnit.CELSIUS);

            Assert.That(temp1.Equals(temp2), Is.False);
        }

        /// <summary>
        /// Tests round-trip conversion accuracy.
        /// Converting from Celsius to Fahrenheit and back should yield original value.
        /// </summary>
        [Test]
        public void testTemperature_RoundTripConversion_Celsius()
        {
            var original = new Quantity<TemperatureUnit>(25, TemperatureUnit.CELSIUS);
            var fahrenheit = original.ConvertTo(TemperatureUnit.FAHRENHEIT);
            var backToCelsius = fahrenheit.ConvertTo(TemperatureUnit.CELSIUS);

            Assert.That(backToCelsius.Value, Is.EqualTo(original.Value).Within(EPSILON));
        }

        /// <summary>
        /// Tests round-trip conversion accuracy.
        /// Converting from Fahrenheit to Kelvin and back should yield original value.
        /// </summary>
        [Test]
        public void testTemperature_RoundTripConversion_Fahrenheit()
        {
            var original = new Quantity<TemperatureUnit>(77, TemperatureUnit.FAHRENHEIT);
            var kelvin = original.ConvertTo(TemperatureUnit.KELVIN);
            var backToFahrenheit = kelvin.ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.That(backToFahrenheit.Value, Is.EqualTo(original.Value).Within(EPSILON));
        }

        /// <summary>
        /// Tests round-trip conversion accuracy.
        /// Converting from Kelvin to Celsius and back should yield original value.
        /// </summary>
        [Test]
        public void testTemperature_RoundTripConversion_Kelvin()
        {
            var original = new Quantity<TemperatureUnit>(298.15, TemperatureUnit.KELVIN);
            var celsius = original.ConvertTo(TemperatureUnit.CELSIUS);
            var backToKelvin = celsius.ConvertTo(TemperatureUnit.KELVIN);

            Assert.That(backToKelvin.Value, Is.EqualTo(original.Value).Within(EPSILON));
        }
    }
}