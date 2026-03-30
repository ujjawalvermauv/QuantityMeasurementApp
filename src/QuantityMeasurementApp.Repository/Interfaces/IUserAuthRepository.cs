using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public interface IUserAuthRepository
    {
        UserAccountEntity? GetByEmail(string email);
        UserAccountEntity Create(UserAccountEntity user);
    }
}
