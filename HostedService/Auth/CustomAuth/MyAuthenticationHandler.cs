using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CustomAuth;

public class MyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "MyAuthentication";

    public const string HttpHeader = "AuthenticationKey";
}

public class MyAuthenticationHandler : AuthenticationHandler<MyAuthenticationOptions>
{
    public MyAuthenticationHandler(IOptionsMonitor<MyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(MyAuthenticationOptions.HttpHeader, out var values)
            || values.Count == 0)
        {
            var result = AuthenticateResult.Fail($"Missing key in header {MyAuthenticationOptions.HttpHeader}");
            return Task.FromResult(result);
        }

        var key = values.ToString();
        if (key != "my-secret-key")
        {
            var result = AuthenticateResult.Fail($"Invalid key in header {MyAuthenticationOptions.HttpHeader}");
            return Task.FromResult(result);
        }

        var claims = new Claim[]
        {
            new Claim("Name", "AuthenticatedUser")
        };
        /*
         * NOTE
         *
         * An authentication type parameter for the CTOR of ClaimsIdentity must be specified, or the new identity
         * will have property "IsAuthenticated" being false, that is, being as not authenticated.
         */
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

public static class MyAuthenticationHandlerServiceCollectionExtensions
{
    public static IServiceCollection AddMyAuthentication(this IServiceCollection services, string? scheme = null)
    {
        services.AddAuthentication()
            .AddScheme<MyAuthenticationOptions, MyAuthenticationHandler>(
                scheme ?? MyAuthenticationOptions.DefaultScheme, 
                _ => { });
        return services;
    }
}
