using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using x3rt.DiscordOAuth2.Entities;
using x3rt.DiscordOAuth2.Options;

namespace x3rt.DiscordOAuth2;

public class DiscordOAuth
{
    private static ulong ClientId { get; set; }
    private static string ClientSecret { get; set; } = string.Empty;
    private static string? BotToken { get; set; }

    private string RedirectUri { get; set; }
    private bool Prompt { get; set; }
    private ScopesBuilder Scopes { get; set; }

    private string? AccessToken { get; set; }

    public static void Configure(ulong clientId, string clientSecret, string? botToken = null)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        BotToken = botToken;
    }

    private readonly HttpClient _httpClient = new HttpClient();

    public DiscordOAuth(string redirectUri, ScopesBuilder scopes, bool prompt = true)
    {
        RedirectUri = redirectUri;
        Scopes = scopes;
        Prompt = prompt;
    }

    public string GetAuthorizationUrl(string state)
    {
        NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
        query["client_id"] = ClientId.ToString();
        query["redirect_uri"] = RedirectUri;
        query["response_type"] = "code";
        query["scope"] = Scopes.ToString();
        query["state"] = state;
        query["prompt"] = Prompt ? "consent" : "none";

        var uriBuilder = new UriBuilder("https://discord.com/api/oauth2/authorize")
        {
            Query = query.ToString()
        };

        return uriBuilder.ToString();
    }

    public static bool TryGetCode(HttpRequest request, out string? code)
    {
        code = null;
        if (request.Query.TryGetValue("code", out StringValues codeValues))
        {
            code = codeValues;
            return true;
        }

        return false;
    }

    public static bool TryGetCode(HttpContext context, out string? code)
    {
        var b = TryGetCode(context.Request, out var a);
        code = a;
        return b;
    }

    public async Task<OAuthToken?> GetTokenAsync(string code)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", ClientId.ToString() },
            { "client_secret", ClientSecret },
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", RedirectUri },
            { "scope", Scopes.ToString() }
        });

        var response = await _httpClient.PostAsync("https://discord.com/api/oauth2/token", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var authToken = JsonConvert.DeserializeObject<OAuthToken>(responseString);
        AccessToken = authToken?.AccessToken;
        return authToken;
    }

    private async Task<T?> GetInformationAsync<T>(string accessToken, string endpoint) where T : class
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.GetAsync($"https://discord.com/api/{endpoint}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T?>(responseString) ?? null;
    }

    private async Task<T?> GetInformationAsync<T>(HttpContext context, string endpoint) where T : class
    {
        if (AccessToken is null)
        {
            if (!TryGetCode(context, out var code)) return null;
            var accessToken = await GetTokenAsync(code!);
            if (accessToken is null) return null;
            return await GetInformationAsync<T>(accessToken.AccessToken, endpoint);
        }
        else
        {
            return await GetInformationAsync<T>(AccessToken, endpoint);
        }
    }

    private async Task<T?> GetInformationAsync<T>(OAuthToken token, string endpoint) where T : class
    {
        return await GetInformationAsync<T>(token.AccessToken, endpoint);
    }

    public async Task<DiscordUser?> GetUserAsync(string accessToken)
    {
        return await GetInformationAsync<DiscordUser>(accessToken, "users/@me");
    }

    public async Task<DiscordUser?> GetUserAsync(HttpContext context)
    {
        return await GetInformationAsync<DiscordUser>(context, "users/@me");
    }

    public async Task<DiscordUser?> GetUserAsync(OAuthToken token)
    {
        return await GetInformationAsync<DiscordUser>(token, "users/@me");
    }

    public async Task<DiscordGuild[]?> GetGuildsAsync(string accessToken)
    {
        return await GetInformationAsync<DiscordGuild[]>(accessToken, "users/@me/guilds");
    }

    public async Task<DiscordGuild[]?> GetGuildsAsync(HttpContext context)
    {
        return await GetInformationAsync<DiscordGuild[]>(context, "users/@me/guilds");
    }

    public async Task<DiscordGuild[]?> GetGuildsAsync(OAuthToken token)
    {
        return await GetInformationAsync<DiscordGuild[]>(token, "users/@me/guilds");
    }

    public async Task<DiscordConnection[]?> GetConnectionsAsync(string accessToken)
    {
        return await GetInformationAsync<DiscordConnection[]>(accessToken, "users/@me/connections");
    }

    public async Task<DiscordConnection[]?> GetConnectionsAsync(HttpContext context)
    {
        return await GetInformationAsync<DiscordConnection[]>(context, "users/@me/connections");
    }

    public async Task<DiscordConnection[]?> GetConnectionsAsync(OAuthToken token)
    {
        return await GetInformationAsync<DiscordConnection[]>(token, "users/@me/connections");
    }

    public async Task<bool> JoinGuildAsync(string accessToken, ulong userId, GuildOptions options)
    {
        if (BotToken is null) throw new InvalidOperationException("Bot token is not set");
        var request =
            new HttpRequestMessage(HttpMethod.Put,
                $"https://discord.com/api/guilds/{options.GuildId}/members/{userId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bot", BotToken);

        var content = new Dictionary<string, object>
        {
            ["access_token"] = accessToken
        };
        if (options.Nickname is not null) content["nick"] = options.Nickname;
        if (options.RoleIds is not null) content["roles"] = options.RoleIds;
        if (options.Muted is not null) content["mute"] = options.Muted;
        if (options.Deafened is not null) content["deaf"] = options.Deafened;

        var json = JsonConvert.SerializeObject(content);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> JoinGuildAsync(HttpContext context, GuildOptions options)
    {
        if (AccessToken is null)
        {
            if (!TryGetCode(context, out var code)) return false;
            var accessToken = await GetTokenAsync(code!);
            if (accessToken is null) return false;
            var user = await GetUserAsync(accessToken.AccessToken);
            if (user is null) return false;
            return await JoinGuildAsync(accessToken.AccessToken, user.Id, options);
        }
        else
        {
            var user = await GetUserAsync(AccessToken);
            if (user is null) return false;
            return await JoinGuildAsync(AccessToken, user.Id, options);
        }
    }

    public async Task<bool> JoinGuildAsync(OAuthToken token, GuildOptions options)
    {
        var user = await GetUserAsync(token.AccessToken);
        if (user is null) return false;
        return await JoinGuildAsync(token.AccessToken, user.Id, options);
    }
}