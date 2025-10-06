namespace AidManager.API.IAM.Interfaces.REST.Resources;

public record SignInResource(string Email, string Password)
{
    public string? CaptchaToken { get; init; } 
}