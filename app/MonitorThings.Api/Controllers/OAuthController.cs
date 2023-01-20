using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using MonitorThings.Api.Configuration;

namespace MonitorThings.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OAuthController : ControllerBase
{
    private readonly ILogger<OAuthController> _logger;
    private readonly ServiceSettings _settings;

    public OAuthController(ILogger<OAuthController> logger, IOptions<ServiceSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    [HttpGet(Name = "GetAuthCallback")]
    public async Task<IActionResult> GetAsync()
    {
        if(Request.Query.ContainsKey("error"))
        {
            var errorDescription = Request.Query["error_description"].FirstOrDefault();
            var error = Request.Query["error"].FirstOrDefault();

            _logger.LogError("Error: {Error}, Description {ErrorDescription}", error, errorDescription);
            return this.BadRequest($"{error}: {errorDescription}");
        }

        var code = Request.Query["code"].FirstOrDefault();
        _logger.LogInformation("Code: {Code}", code);

        var tokenResponse = await GetDiscordTokenResponseAsync(code);
        _logger.LogInformation("AccessToken: {AccessToken}", tokenResponse.AccessToken);
        _logger.LogInformation("RefreshToken: {RefreshToken}", tokenResponse.RefreshToken);
        _logger.LogInformation("ExpiresOn: {ExpiresOn}", tokenResponse.ExpiresOn);

        return Ok();
    }

    private async Task<DiscordAccessTokenResponse> GetDiscordTokenResponseAsync(string code)
    {
        var data = new Dictionary<string, string>
        {
            ["client_id"] = _settings.DiscordClientId,
            ["client_secret"] = _settings.DiscordClientSecret,
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = _settings.RedirectUri,
        };

        using var client = new HttpClient();

        var responseMessage = await client.PostAsync("https://discord.com/api/v10/oauth2/token", new FormUrlEncodedContent(data));

        var response  = JsonSerializer.Deserialize<DiscordAccessTokenResponse>(await responseMessage.Content.ReadAsStringAsync());
        response.ExpiresOn = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn);

        return response;
    }

    private class DiscordAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        public DateTimeOffset ExpiresOn { get; set; }

    }
}
