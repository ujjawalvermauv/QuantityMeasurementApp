using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Business;

namespace QuantityMeasurementApp.Business
{
    public static class ServiceCompatibilityExtensions
    {
        public static bool AreEqual<U>(
            this QuantityMeasurementServiceImpl service,
            Quantity<U> firstMeasurement,
            Quantity<U> secondMeasurement
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(firstMeasurement);
            ArgumentNullException.ThrowIfNull(secondMeasurement);
            return firstMeasurement.Equals(secondMeasurement);
        }

        public static double Convert<U>(
            this QuantityMeasurementServiceImpl service,
            double value,
            U sourceUnit,
            U targetUnit
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            var quantity = new Quantity<U>(value, sourceUnit);
            return quantity.ConvertTo(targetUnit).Value;
        }

        public static Quantity<U> Add<U>(
            this QuantityMeasurementServiceImpl service,
            Quantity<U> firstMeasurement,
            Quantity<U> secondMeasurement
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(firstMeasurement);
            ArgumentNullException.ThrowIfNull(secondMeasurement);
            return firstMeasurement.Add(secondMeasurement);
        }

        public static Quantity<U> Add<U>(
            this QuantityMeasurementServiceImpl service,
            Quantity<U> firstMeasurement,
            Quantity<U> secondMeasurement,
            U targetUnit
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(firstMeasurement);
            ArgumentNullException.ThrowIfNull(secondMeasurement);
            return firstMeasurement.Add(secondMeasurement, targetUnit);
        }

        public static Quantity<U> Add<U>(
            this QuantityMeasurementServiceImpl service,
            double firstValue,
            U firstUnit,
            double secondValue,
            U secondUnit
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            var firstMeasurement = new Quantity<U>(firstValue, firstUnit);
            return firstMeasurement.Add(secondValue, secondUnit);
        }

        public static Quantity<U> Add<U>(
            this QuantityMeasurementServiceImpl service,
            double firstValue,
            U firstUnit,
            double secondValue,
            U secondUnit,
            U targetUnit
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            var firstMeasurement = new Quantity<U>(firstValue, firstUnit);
            return firstMeasurement.Add(secondValue, secondUnit, targetUnit);
        }

        public static Quantity<U> Subtract<U>(
            this QuantityMeasurementServiceImpl service,
            Quantity<U> firstMeasurement,
            Quantity<U> secondMeasurement
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(firstMeasurement);
            ArgumentNullException.ThrowIfNull(secondMeasurement);
            return firstMeasurement.Subtract(secondMeasurement);
        }

        public static Quantity<U> Subtract<U>(
            this QuantityMeasurementServiceImpl service,
            Quantity<U> firstMeasurement,
            Quantity<U> secondMeasurement,
            U targetUnit
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(firstMeasurement);
            ArgumentNullException.ThrowIfNull(secondMeasurement);
            return firstMeasurement.Subtract(secondMeasurement, targetUnit);
        }

        public static Quantity<U> Subtract<U>(
            this QuantityMeasurementServiceImpl service,
            double firstValue,
            U firstUnit,
            double secondValue,
            U secondUnit
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            var firstMeasurement = new Quantity<U>(firstValue, firstUnit);
            return firstMeasurement.Subtract(new Quantity<U>(secondValue, secondUnit));
        }

        public static Quantity<U> Subtract<U>(
            this QuantityMeasurementServiceImpl service,
            double firstValue,
            U firstUnit,
            double secondValue,
            U secondUnit,
            U targetUnit
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            var firstMeasurement = new Quantity<U>(firstValue, firstUnit);
            return firstMeasurement.Subtract(new Quantity<U>(secondValue, secondUnit), targetUnit);
        }

        public static double Divide<U>(
            this QuantityMeasurementServiceImpl service,
            Quantity<U> firstMeasurement,
            Quantity<U> secondMeasurement
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(firstMeasurement);
            ArgumentNullException.ThrowIfNull(secondMeasurement);
            return firstMeasurement.Divide(secondMeasurement);
        }

        public static double Divide<U>(
            this QuantityMeasurementServiceImpl service,
            double firstValue,
            U firstUnit,
            double secondValue,
            U secondUnit
        )
            where U : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(service);
            var firstMeasurement = new Quantity<U>(firstValue, firstUnit);
            var secondMeasurement = new Quantity<U>(secondValue, secondUnit);
            return firstMeasurement.Divide(secondMeasurement);
        }
    }
}
