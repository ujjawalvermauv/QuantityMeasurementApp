using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.QuantityTests
{
    /// <summary>
    /// Tests validating Quantity constructor rules.
    /// </summary>
    [TestFixture]
    public class QuantityConstructorTests
    {
        /// <summary>
        /// Ensures invalid numeric values are rejected.
        /// </summary>
        [Test]
        public void testGenericQuantity_ConstructorValidation_InvalidValue()
        {
            Assert.Throws<ArgumentException>(() =>
                new Quantity<LengthUnit>(double.NaN, LengthUnit.FEET));
        }

        /// <summary>
        /// Default enum value is allowed (null not possible for enums).
        /// </summary>
        [Test]
        public void testGenericQuantity_ConstructorValidation_DefaultEnumAllowed()
        {
            Assert.DoesNotThrow(() =>
                new Quantity<LengthUnit>(10, default));
        }
    }
}