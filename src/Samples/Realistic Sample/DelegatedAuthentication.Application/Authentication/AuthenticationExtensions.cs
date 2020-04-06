using Microsoft.Extensions.DependencyInjection;
using WorldDomination.DelegatedAuthentication.Application.Settings;
using WorldDomination.DelegatedAuthentication.Auth0;
using WorldDomination.DelegatedAuthentication.Domain;

namespace WorldDomination.DelegatedAuthentication.Application.Authentication
{
    internal static class AuthenticationExtensions
    {
        internal static IServiceCollection AddCustomAuthentication(this IServiceCollection services,
                                                                   AuthenticationSettings settings)
        {
            var authenticationService = new AuthenticationService<Auth0Jwt, CustomJwt, CustomAuthenticationOptions, Account>(settings.Auth0Secret,
                                                                                                                             settings.CustomSecret);
            services.AddSingleton<IAuthenticationService<Auth0Jwt, CustomJwt, CustomAuthenticationOptions, Account>>(authenticationService);

            services.AddCustomJwtAuthentication(settings.CustomAudience,
                                                settings.CustomAuthority,
                                                settings.CustomSecret);

            return services;
        }

    }
}
