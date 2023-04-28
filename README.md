# x3rt.DiscordOAuth2

[![NuGet](https://img.shields.io/nuget/v/x3rt.DiscordOAuth2.svg)](https://www.nuget.org/packages/x3rt.DiscordOAuth2/)
[![NuGet](https://img.shields.io/nuget/dt/x3rt.DiscordOAuth2.svg)](https://www.nuget.org/packages/x3rt.DiscordOAuth2/)

A **simple** library to handle Discord OAuth2 authentication.
Meant to serve as an alternative to
the [AspNet.Security.OAuth.Providers](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers) library that
gives more control to the user.

## Usage

```csharp
// Configure OAuth ClientID and ClientSecret globally
DiscordOAuth.Configure(0123456789, "ClientSecret", "OptionalBotToken");
```
```csharp
var scopes = new ScopesBuilder(OAuthScope.Identify);
var oAuth = new DiscordOAuth("https://example.com/Login", scopes);
var url = oAuth.GetAuthorizationUrl("state");
/* Redirect user to url via preferred method */
```
```csharp
// Your callback method
if (DiscordOAuth.TryGetCode(HttpContext, out var code))
{
    var scopes = new ScopesBuilder(OAuthScope.Identify);
    var oAuth = new DiscordOAuth("https://example.com/Login", scopes);
    var token = await oAuth.GetTokenAsync(code);
    var user = await oAuth.GetUserAsync(token);
    
    ulong userId = user.Id;
    // ...
    
}
```

## Feedback

DiscordOAuth2 is a work in progress and any feedback or suggestions are welcome.
This library is released under the [MIT License](LICENSE).