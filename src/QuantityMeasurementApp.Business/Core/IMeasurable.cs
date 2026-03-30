namespace QuantityMeasurementApp.Business
{
    public delegate bool SupportsArithmetic();

    public interface IMeasurable
    {
        double GetConversionFactor();
        double ConvertToBaseUnit(double value);
        double ConvertFromBaseUnit(double baseValue);
        string GetUnitName();

        public bool SupportsArithmetic()
        {
            SupportsArithmetic supportsArithmetic = () => true;
            return supportsArithmetic();
        }

        public void ValidateOperationSupport(string operation) { }
    }
}
