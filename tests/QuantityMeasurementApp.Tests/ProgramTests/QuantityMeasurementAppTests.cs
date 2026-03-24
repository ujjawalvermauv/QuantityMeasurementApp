using NUnit.Framework;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;

namespace QuantityMeasurementApp.Tests.ProgramTests
{
    /// <summary>
    /// Tests verifying generic demonstration behavior.
    /// </summary>
    [TestFixture]
    public class QuantityMeasurementAppTests
    {
        [Test]
        public void testQuantityMeasurementApp_SimplifiedDemonstration_Equality()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testQuantityMeasurementApp_SimplifiedDemonstration_Conversion()
        {
            var q = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);

            var result = q.ConvertTo(WeightUnit.GRAM);

            Assert.That(result.Value, Is.EqualTo(1000));
        }

        [Test]
        public void testQuantityMeasurementApp_SimplifiedDemonstration_Addition()
        {
            var q1 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

            var result = q1.Add(q2, WeightUnit.KILOGRAM);

            Assert.That(result.Value, Is.EqualTo(2));
        }
    }
}