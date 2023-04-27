using Newtonsoft.Json;

namespace x3rt.DiscordOAuth2.Entities;

public class OAuthToken
{
    [JsonProperty("access_token")] public string AccessToken { get; set; }

    [JsonProperty("expires_in")] public int ExpiresIn { get; set; }

    [JsonProperty("refresh_token")] public string RefreshToken { get; set; }

    [JsonProperty("scope")] public string Scope { get; set; }

    [JsonProperty("token_type")] public string TokenType { get; set; }
}