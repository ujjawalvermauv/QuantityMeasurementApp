using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Api.Contracts;

public class GoogleLoginRequestDto
{
    [Required]
    public string IdToken { get; set; } = string.Empty;
}
