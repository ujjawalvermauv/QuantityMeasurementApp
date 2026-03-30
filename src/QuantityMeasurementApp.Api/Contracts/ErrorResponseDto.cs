namespace QuantityMeasurementApp.Api.Contracts;

public class ErrorResponseDto
{
    public DateTime Timestamp { get; set; }
    public int Status { get; set; }
    public string Error { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Path { get; set; }
}
