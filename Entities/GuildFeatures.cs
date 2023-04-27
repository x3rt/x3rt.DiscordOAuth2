using x3rt.DiscordOAuth2.Entities.Enums;

namespace x3rt.DiscordOAuth2.Entities;

public record GuildFeatures
{
    IEnumerable<GuildFeature> Features { get; set; }

    public override string ToString()
    {
        return string.Join(", ", Features);
    }
}