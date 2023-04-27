namespace x3rt.DiscordOAuth2.Options;

public class GuildOptions
{
    public ulong GuildId { get; set; }
    public string? Nickname { get; set; }
    public IEnumerable<ulong>? RoleIds { get; set; }
    public bool? Muted { get; set; }
    public bool? Deafened { get; set; }

    public GuildOptions(ulong guildId, string? nickname = null, IEnumerable<ulong>? roleIds = null, bool? mute = null, bool? deafened = null)
    {
        GuildId = guildId;
        Nickname = nickname;
        RoleIds = roleIds;
        Muted = mute;
        Deafened = deafened;
    }
    
    public GuildOptions Mute(bool mute = true)
    {
        Muted = mute;
        return this;
    }
    
    public GuildOptions Deafen(bool deafen = true)
    {
        Deafened = deafen;
        return this;
    }

    public GuildOptions WithNickname(string nickname)
    {
        Nickname = nickname;
        return this;
    }

    public GuildOptions WithRoleIds(params ulong[] roleIds)
    {
        RoleIds = roleIds;
        return this;
    }

    public GuildOptions WithRoleIds(IEnumerable<ulong> roleIds)
    {
        RoleIds = roleIds.ToArray();
        return this;
    }

    public GuildOptions WithRoleId(ulong roleId)
    {
        if (RoleIds is null)
        {
            RoleIds = new[] { roleId };
        }
        else
        {
            RoleIds = RoleIds.Append(roleId);
        }

        return this;
    }


    public override string ToString()
    {
        return $"GuildId: {GuildId}; Nickname: {Nickname}; RoleIds: {RoleIds}";
    }
}