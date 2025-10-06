namespace AidManager.API.IAM.Application.Options;

public sealed class ReCaptchaOptions
{
    public string SiteKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}