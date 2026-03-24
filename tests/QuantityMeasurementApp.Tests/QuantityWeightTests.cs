using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Legacy test file for weight quantity operations using generic Quantity<WeightUnit>.
    /// Demonstrates backward compatibility with UC9 test patterns.
    /// </summary>
    [TestFixture]
    public class QuantityWeightLegacyTests
    {
        private const double EPSILON = 1e-6;

        [Test]
        public void testEquality_KilogramToGram()
        {
            var a = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var b = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void testConversion_KilogramToGram()
        {
            var result = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM)
                .ConvertTo(WeightUnit.GRAM);

            Assert.That(result.Value, Is.EqualTo(1000).Within(EPSILON));
        }

        [Test]
        public void testAddition_KgPlusGram()
        {
            var result = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM)
                .Add(new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM));

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
        }

        [Test]
        public void testAddition_TargetUnit()
        {
            var result = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM)
                .Add(new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM), WeightUnit.GRAM);

            Assert.That(result.Value, Is.EqualTo(2000).Within(EPSILON));
        }
    }
}