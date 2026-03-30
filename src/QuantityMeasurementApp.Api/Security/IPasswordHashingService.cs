namespace QuantityMeasurementApp.Api.Security;

public interface IPasswordHashingService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string storedHash);
}
