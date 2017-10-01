using System;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace WorldDomination.DelegatedAuthentication.WebApi
{
    internal static class CustomJwtAuthenticationExtensions
    {
        public static AuthenticationBuilder AddCustomJwtAuthentication(this IServiceCollection services,
                                                                       string audience,
                                                                       string issuer,
                                                                       string secret)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new ArgumentException(nameof(audience));
            }

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new ArgumentException(nameof(issuer));
            }

            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentException(nameof(secret));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var tokenValidationParameters = new TokenValidationParameters
            {
                // Signing key must match.
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Iss claim.
                ValidateIssuer = true,
                ValidIssuer = issuer,

                // Aud claim
                ValidateAudience = true,
                ValidAudience = audience
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; });

            return new AuthenticationBuilder(services);
        }
    }
}