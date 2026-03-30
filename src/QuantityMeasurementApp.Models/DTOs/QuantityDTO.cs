namespace QuantityMeasurementApp.Models.DTOs
{
    public enum MeasurementCategory
    {
        Length,
        Weight,
        Volume,
        Temperature,
        Unknown,
    }

    public class QuantityDTO
    {
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public MeasurementCategory Category { get; set; }

        public QuantityDTO() { }

        public QuantityDTO(double value, string unit, MeasurementCategory category)
        {
            Value = value;
            Unit = unit;
            Category = category;
        }
    }
}
