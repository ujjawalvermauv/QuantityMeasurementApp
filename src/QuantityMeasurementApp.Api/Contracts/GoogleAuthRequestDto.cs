using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Api.Contracts;

public class GoogleAuthRequestDto
{
    [Required]
    public string IdToken { get; set; } = string.Empty;
}