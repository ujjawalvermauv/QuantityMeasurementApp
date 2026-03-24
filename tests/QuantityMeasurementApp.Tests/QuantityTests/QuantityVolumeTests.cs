using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.QuantityTests
{
    /// <summary>
    /// Tests for volume-based quantity operations.
    /// Validates UC11 implementation with generic Quantity<VolumeUnit>.
    /// </summary>
    [TestFixture]
    public class QuantityVolumeTests
    {
        private const double EPSILON = 1e-5;

        /// <summary>
        /// Tests equality between equivalent volume quantities.
        /// </summary>
        [Test]
        public void testGenericQuantity_VolumeOperations_Equality()
        {
            var q1 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(1000, VolumeUnit.MILLILITRE);

            Assert.That(q1.Equals(q2), Is.True);
        }

        /// <summary>
        /// Tests conversion between volume units.
        /// </summary>
        [Test]
        public void testGenericQuantity_VolumeOperations_Conversion()
        {
            var q = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);

            var result = q.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.That(result.Value, Is.EqualTo(1000));
        }

        /// <summary>
        /// Tests addition of volume quantities.
        /// </summary>
        [Test]
        public void testGenericQuantity_VolumeOperations_Addition()
        {
            var q1 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var q2 = new Quantity<VolumeUnit>(1000, VolumeUnit.MILLILITRE);

            var result = q1.Add(q2, VolumeUnit.LITRE);

            Assert.That(result.Value, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests cross-unit equality with gallons.
        /// </summary>
        [Test]
        public void testVolumeEquality_LitreToGallon()
        {
            var litre = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);
            var gallon = new Quantity<VolumeUnit>(1, VolumeUnit.GALLON);

            Assert.That(litre.Equals(gallon), Is.True);
        }

        /// <summary>
        /// Tests conversion from gallon to litre.
        /// </summary>
        [Test]
        public void testVolumeConversion_GallonToLitre()
        {
            var gallon = new Quantity<VolumeUnit>(1, VolumeUnit.GALLON);

            var result = gallon.ConvertTo(VolumeUnit.LITRE);

            Assert.That(result.Value, Is.EqualTo(3.79).Within(EPSILON));
        }

        /// <summary>
        /// Tests addition with explicit target unit.
        /// </summary>
        [Test]
        public void testVolumeAddition_ExplicitTarget()
        {
            var litre = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var gallon = new Quantity<VolumeUnit>(1, VolumeUnit.GALLON);

            var result = litre.Add(gallon, VolumeUnit.LITRE);

            Assert.That(result.Value, Is.EqualTo(4.79).Within(EPSILON));
        }
    }
}