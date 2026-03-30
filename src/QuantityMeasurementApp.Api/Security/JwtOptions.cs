namespace QuantityMeasurementApp.Api.Security;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "QuantityMeasurementApp";
    public string Audience { get; set; } = "QuantityMeasurementApp.Client";
    public string SecretKey { get; set; } = "REPLACE_WITH_A_LONG_RANDOM_SECRET_KEY_FOR_PRODUCTION_32_CHARS_MIN";
    public int ExpiryMinutes { get; set; } = 60;
}
