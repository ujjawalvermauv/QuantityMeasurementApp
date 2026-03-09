namespace QuantityMeasurementApp
{
    /// <summary>
    /// UC10: Backward compatibility wrapper for QuantityWeight.
    /// This class provides compatibility with existing code while using the generic Quantity<WeightUnit> implementation.
    /// </summary>
    public class QuantityWeight : Quantity<WeightUnit>
    {
        public QuantityWeight(double value, WeightUnit unit) : base(value, unit)
        {
        }
    }
}
