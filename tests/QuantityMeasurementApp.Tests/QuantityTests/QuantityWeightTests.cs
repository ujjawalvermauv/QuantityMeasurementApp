using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.QuantityTests
{
    /// <summary>
    /// Tests for weight-based quantity operations.
    /// </summary>
    [TestFixture]
    public class QuantityWeightTests
    {
        /// <summary>
        /// Tests equality of equivalent weights.
        /// </summary>
        [Test]
        public void testGenericQuantity_WeightOperations_Equality()
        {
            var w1 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

            Assert.That(w1.Equals(w2), Is.True);
        }

        /// <summary>
        /// Tests conversion between weight units.
        /// </summary>
        [Test]
        public void testGenericQuantity_WeightOperations_Conversion()
        {
            var w = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);

            var result = w.ConvertTo(WeightUnit.GRAM);

            Assert.That(result.Value, Is.EqualTo(1000));
        }

        /// <summary>
        /// Tests addition of weight quantities.
        /// </summary>
        [Test]
        public void testGenericQuantity_WeightOperations_Addition()
        {
            var w1 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

            var result = w1.Add(w2, WeightUnit.KILOGRAM);

            Assert.That(result.Value, Is.EqualTo(2));
        }
    }
}