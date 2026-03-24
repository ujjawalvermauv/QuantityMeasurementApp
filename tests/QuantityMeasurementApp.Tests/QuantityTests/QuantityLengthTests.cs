using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.QuantityTests
{
    /// <summary>
    /// Tests for length-based quantity operations.
    /// </summary>
    [TestFixture]
    public class QuantityLengthTests
    {
        /// <summary>
        /// Tests equality between equivalent length quantities.
        /// </summary>
        [Test]
        public void testGenericQuantity_LengthOperations_Equality()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

            Assert.That(q1.Equals(q2), Is.True);
        }

        /// <summary>
        /// Tests conversion between length units.
        /// </summary>
        [Test]
        public void testGenericQuantity_LengthOperations_Conversion()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            var result = q.ConvertTo(LengthUnit.INCHES);

            Assert.That(result.Value, Is.EqualTo(12));
        }

        /// <summary>
        /// Tests addition of length quantities.
        /// </summary>
        [Test]
        public void testGenericQuantity_LengthOperations_Addition()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

            var result = q1.Add(q2, LengthUnit.FEET);

            Assert.That(result.Value, Is.EqualTo(2));
        }
    }
}