using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Validates weight conversion constants and base-unit transforms.
    /// </summary>
    [TestClass]
    public class WeightUnitTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void WeightUnitEnum_KilogramConstant()
        {
            Assert.AreEqual(1.0, WeightUnit.Kilogram.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        public void WeightUnitEnum_GramConstant()
        {
            Assert.AreEqual(0.001, WeightUnit.Gram.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        public void WeightUnitEnum_PoundConstant()
        {
            Assert.AreEqual(0.453592, WeightUnit.Pound.GetConversionFactor(), Epsilon);
        }

        [TestMethod]
        public void ConvertToBaseUnit_GramToKilogram()
        {
            Assert.AreEqual(1.0, WeightUnit.Gram.ConvertToBaseUnit(1000.0), Epsilon);
        }

        [TestMethod]
        public void ConvertToBaseUnit_PoundToKilogram()
        {
            Assert.AreEqual(0.907184, WeightUnit.Pound.ConvertToBaseUnit(2.0), Epsilon);
        }

        [TestMethod]
        public void ConvertFromBaseUnit_KilogramToGram()
        {
            Assert.AreEqual(1000.0, WeightUnit.Gram.ConvertFromBaseUnit(1.0), Epsilon);
        }

        [TestMethod]
        public void ConvertFromBaseUnit_KilogramToPound()
        {
            Assert.AreEqual(2.2046244201837775, WeightUnit.Pound.ConvertFromBaseUnit(1.0), Epsilon);
        }
    }
}
