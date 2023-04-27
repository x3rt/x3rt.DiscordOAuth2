using Newtonsoft.Json;

namespace x3rt.DiscordOAuth2.Entities;

public class DiscordConnection
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("type")] public ConnectionType Type { get; set; }
    [JsonProperty("revoked")] public bool? Revoked { get; set; }
    [JsonProperty("integrations")] public object[] Integrations { get; set; }
    [JsonProperty("verified")] public bool? Verified { get; set; }
    [JsonProperty("friend_sync")] public bool? FriendSync { get; set; }
    [JsonProperty("show_activity")] public bool? ShowActivity { get; set; }
    [JsonProperty("two_way_link")] public bool? TwoWayLink { get; set; }
    [JsonProperty("visibility")] public ConnectionVisibility? Visibility { get; set; }


    public enum ConnectionVisibility
    {
        None,
        Everyone
    }


    public enum ConnectionType
    {
        BattleNet,
        Ebay,
        EpicGames,
        Facebook,
        GitHub,
        Instagram,
        LeagueOfLegends,
        PayPal,
        PlayStation,
        Reddit,
        RiotGames,
        Spotify,
        Skype,
        Steam,
        TikTok,
        Twitch,
        Twitter,
        Xbox,
        YouTube
    }
}