using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Api.Contracts;

public class SignupRequestDto
{
    [Required]
    [MinLength(2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}
