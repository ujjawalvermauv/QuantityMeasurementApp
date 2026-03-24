using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.QuantityTests
{
    /// <summary>
    /// Tests preventing cross-category comparisons.
    /// </summary>
    [TestFixture]
    public class QuantityCrossCategoryTests
    {
        /// <summary>
        /// Ensures length and weight quantities cannot be equal.
        /// </summary>
        [Test]
        public void testCrossCategoryPrevention_LengthVsWeight()
        {
            var length = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var weight = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);

            Assert.That(length.Equals(weight), Is.False);
        }
    }
}