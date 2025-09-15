using AidManager.API.IAM.Domain.Model.Aggregates;

namespace AidManager.API.IAM.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(UserAuth user);
    Task<int?> ValidateToken(string token);
}