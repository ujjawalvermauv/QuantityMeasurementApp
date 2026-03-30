using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Business
{
    public interface IQuantityMeasurementService
    {
        QuantityDTO Convert(QuantityDTO source, string targetUnit);
        bool Compare(QuantityDTO first, QuantityDTO second);
        QuantityDTO Add(QuantityDTO a, QuantityDTO b, string? targetUnit = null);
        QuantityDTO Subtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null);
        double Divide(QuantityDTO a, QuantityDTO b);
    }
}
