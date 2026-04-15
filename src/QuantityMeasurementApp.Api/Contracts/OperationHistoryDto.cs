namespace QuantityMeasurementApp.Api.Contracts;

public class OperationHistoryDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
