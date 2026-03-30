using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Api.Contracts;

public class ConvertOperationRequestDto
{
    [Required]
    public ApiQuantityDto Source { get; set; } = new();

    [Required]
    public string TargetUnit { get; set; } = string.Empty;
}
