using System.Text.Json.Serialization;

namespace AidManager.API.IAM.Application.Models;

public sealed class ReCaptchaVerifyResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("challenge_ts")] public DateTimeOffset? ChallengeTs { get; set; }
    [JsonPropertyName("hostname")] public string Hostname { get; set; } = string.Empty;
    [JsonPropertyName("error-codes")] public string[]? ErrorCodes { get; set; }
}