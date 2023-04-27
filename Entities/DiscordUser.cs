using x3rt.DiscordOAuth2.Entities.Enums;

namespace x3rt.DiscordOAuth2.Entities;

public class DiscordUser
{
    public ulong Id { get; set; }
    public string Username { get; set; }
    public string Discriminator { get; set; }
    public string? Avatar { get; set; }
    public bool? Bot { get; set; }
    public bool? System { get; set; }
    public bool? MfaEnabled { get; set; }
    public string? Banner { get; set; }
    public int? AccentColor { get; set; }
    public string? Locale { get; set; }
    public bool? Verified { get; set; }
    public string? Email { get; set; }
    public UserFlag? Flags { get; set; }
    public PremiumType? PremiumType { get; set; }
    public UserFlag? PublicFlags { get; set; }

    public override string ToString()
    {
        string result = "";
        foreach (var property in GetType().GetProperties())
        {
            var value = property.GetValue(this);
            if (value is not null)
            {
                result += $"{property.Name}: {value}; ";
            }
        }

        result = result.TrimEnd(' ', ';');
        return result;
    }
}