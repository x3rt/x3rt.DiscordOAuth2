using x3rt.DiscordOAuth2.Entities.Enums;

namespace x3rt.DiscordOAuth2;

public class ScopesBuilder
{
    List<OAuthScope> _scopes = new List<OAuthScope>();


    public ScopesBuilder()
    {
    }

    public ScopesBuilder(OAuthScope scope)
    {
        _scopes.Add(scope);
    }
    
    public ScopesBuilder(IEnumerable<OAuthScope> scopes)
    {
        _scopes.AddRange(scopes);
    }
    
    public ScopesBuilder(params OAuthScope[] scopes)
    {
        _scopes.AddRange(scopes);
    }

    public ScopesBuilder AddScope(OAuthScope scope)
    {
        _scopes.Add(scope);
        return this;
    }

    public ScopesBuilder AddScopes(IEnumerable<OAuthScope> scopes)
    {
        _scopes.AddRange(scopes);
        return this;
    }

    public ScopesBuilder AddScopes(params OAuthScope[] scopes)
    {
        _scopes.AddRange(scopes);
        return this;
    }

    public string Build()
    {
        return string.Join(" ", _scopes.Select(TranslateOAuthScope));
    }

    public override string ToString()
    {
        return Build();
    }


    private string? TranslateOAuthScope(OAuthScope scope)
    {
        return scope switch
        {
            OAuthScope.Identify => "identify",
            OAuthScope.Email => "email",
            OAuthScope.Connections => "connections",
            OAuthScope.Guilds => "guilds",
            OAuthScope.GuildsJoin => "guilds.join",
            OAuthScope.GuildsMembersRead => "guilds.members.read",
            OAuthScope.GdmJoin => "gdm.join",
            OAuthScope.Rpc => "rpc",
            OAuthScope.RpcNotificationsRead => "rpc.notifications.read",
            OAuthScope.RpcVoiceRead => "rpc.voice.read",
            OAuthScope.RpcVoiceWrite => "rpc.voice.write",
            OAuthScope.RpcActivitiesWrite => "rpc.activities.write",
            OAuthScope.Bot => "bot",
            OAuthScope.WebhookIncoming => "webhook.incoming",
            OAuthScope.MessagesRead => "messages.read",
            OAuthScope.ApplicationsBuildsUpload => "applications.builds.upload",
            OAuthScope.ApplicationsBuildsRead => "applications.builds.read",
            OAuthScope.ApplicationsCommands => "applications.commands",
            OAuthScope.ApplicationsStoreUpdate => "applications.store.update",
            OAuthScope.ApplicationsEntitlements => "applications.entitlements",
            OAuthScope.ActivitiesRead => "activities.read",
            OAuthScope.ActivitiesWrite => "activities.write",
            OAuthScope.RelationshipsRead => "relationships.read",
            _ => null
        };
    }
}

