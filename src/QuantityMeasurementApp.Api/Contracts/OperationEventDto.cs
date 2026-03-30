namespace QuantityMeasurementApp.Api.Contracts;

public class OperationEventDto
{
    public string Operation { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public string? Category { get; set; }
    public string? Unit { get; set; }
}
