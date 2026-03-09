namespace QuantityMeasurementApp
{
    /// <summary>
    /// UC10: Backward compatibility wrapper for QuantityLength.
    /// This class provides compatibility with existing code while using the generic Quantity<LengthUnit> implementation.
    /// </summary>
    public class QuantityLength : Quantity<LengthUnit>
    {
        public QuantityLength(double value, LengthUnit unit) : base(value, unit)
        {
        }
    }
}
