using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Api.Contracts;

public class BinaryOperationRequestDto
{
    [Required]
    public ApiQuantityDto First { get; set; } = new();

    [Required]
    public ApiQuantityDto Second { get; set; } = new();

    public string? TargetUnit { get; set; }
}
