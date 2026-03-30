using QuantityMeasurementApp.Api.Contracts;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Api.Security;

public interface IJwtTokenService
{
    AuthResponseDto GenerateToken(UserAccountEntity user);
}
