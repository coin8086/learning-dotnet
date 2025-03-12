//See https://learn.microsoft.com/en-us/aspnet/core/security/authentication/certauth
//and https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints

using Microsoft.AspNetCore.Authentication.Certificate;
using System.Security.Cryptography.X509Certificates;

namespace CertAuth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
            .AddCertificate(options =>
            {
                //options.ValidateCertificateUse = false;
                options.ChainTrustValidationMode = X509ChainTrustMode.CustomRootTrust;
                //options.CustomTrustStore = 
            });

        var app = builder.Build();
        app.UseAuthentication();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}
