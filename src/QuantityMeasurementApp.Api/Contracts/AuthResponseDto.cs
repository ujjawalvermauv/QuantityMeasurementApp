namespace QuantityMeasurementApp.Api.Contracts;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Message { get; set; } = "Authentication successful";
    public bool Success { get; set; } = true;
}
