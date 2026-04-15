using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Api.Contracts;

public class OperationResultDto
{
    public string Operation { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ApiQuantityDto? First { get; set; }
    public ApiQuantityDto? Second { get; set; }
    public QuantityDTO? QuantityResult { get; set; }
    public bool? BooleanResult { get; set; }
    public double? ScalarResult { get; set; }
    public bool Error { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public string Message { get; set; } = "Operation completed successfully";
}
