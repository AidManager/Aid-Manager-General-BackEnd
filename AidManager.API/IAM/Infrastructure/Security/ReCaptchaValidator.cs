using AidManager.API.Authentication.Application.Models;
using AidManager.API.IAM.Application.Options;
using AidManager.API.IAM.Interfaces.Security;
using Microsoft.Extensions.Options;

namespace AidManager.API.IAM.Infrastructure.Security;

public sealed class ReCaptchaValidator : IReCaptchaValidator
{
    private readonly HttpClient _http;
    private readonly ReCaptchaOptions _opt;

    public ReCaptchaValidator(HttpClient http, IOptions<ReCaptchaOptions> opt)
    {
        _http = http;
        _http.Timeout = TimeSpan.FromSeconds(5);
        _opt = opt.Value;
    }

    public async Task<bool> ValidateAsync(string token, string? remoteIp = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;

        using var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string,string>("secret", _opt.SecretKey),
            new KeyValuePair<string,string>("response", token),
            new KeyValuePair<string,string>("remoteip", remoteIp ?? string.Empty),
        });

        using var resp = await _http.PostAsync("https://www.google.com/recaptcha/api/siteverify", content, ct);
        if (!resp.IsSuccessStatusCode) return false;

        var payload = await resp.Content.ReadFromJsonAsync<ReCaptchaVerifyResponse>(cancellationToken: ct);
        return payload?.Success == true;
    }
}