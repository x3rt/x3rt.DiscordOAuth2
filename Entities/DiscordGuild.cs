using Newtonsoft.Json;

namespace x3rt.DiscordOAuth2.Entities;

public class DiscordGuild
{
    [JsonProperty("id")] public ulong Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("icon")] public string? Icon { get; set; }

    [JsonProperty("owner")] public bool Owner { get; set; }

    [JsonProperty("permissions")] public string Permissions { get; set; }

    [JsonProperty("features")] public GuildFeatures Features { get; set; }

    public override string ToString()
    {
        return
            $"Id: {Id}; Name: {Name}; Icon: {Icon}; Owner: {Owner}; Permissions: {Permissions}; Features: {Features}";
    }
}