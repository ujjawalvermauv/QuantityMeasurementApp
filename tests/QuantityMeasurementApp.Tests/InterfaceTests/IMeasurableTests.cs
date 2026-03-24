using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.InterfaceTests
{
    /// <summary>
    /// Tests verifying that unit enums correctly implement measurable behavior.
    /// </summary>
    [TestFixture]
    public class IMeasurableTests
    {
        /// <summary>
        /// Verifies LengthUnit conversion behavior.
        /// </summary>
        [Test]
        public void testIMeasurableInterface_LengthUnitImplementation()
        {
            double result = LengthUnit.FEET.ConvertToBaseUnit(1);

            Assert.That(result, Is.EqualTo(12));
        }

        /// <summary>
        /// Verifies WeightUnit conversion behavior.
        /// </summary>
        [Test]
        public void testIMeasurableInterface_WeightUnitImplementation()
        {
            double result = WeightUnit.KILOGRAM.ConvertToBaseUnit(1);

            Assert.That(result, Is.EqualTo(1000));
        }

        /// <summary>
        /// Verifies consistent conversion behavior between units.
        /// </summary>
        [Test]
        public void testIMeasurableInterface_ConsistentBehavior()
        {
            double lengthBase = LengthUnit.INCHES.ConvertToBaseUnit(10);
            double weightBase = WeightUnit.GRAM.ConvertToBaseUnit(10);

            Assert.That(lengthBase, Is.EqualTo(10));
            Assert.That(weightBase, Is.EqualTo(10));
        }
    }
}