namespace AidManager.API.IAM.Interfaces.Security;

public interface IReCaptchaValidator
{
    Task<bool> ValidateAsync(string token, string? remoteIp = null, CancellationToken ct = default);
}