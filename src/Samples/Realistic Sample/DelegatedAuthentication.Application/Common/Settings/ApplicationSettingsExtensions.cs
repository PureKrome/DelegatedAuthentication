using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WorldDomination.DelegatedAuthentication.Application.Settings;

namespace WorldDomination.DelegatedAuthentication.Application.Common.Settings
{
    public static class ApplicationSettingsExtensions
    {
        public static AuthenticationSettings AddApplicationSettings(this IServiceCollection services,
                                                                    IConfiguration configuration)
        {
            var authenticationSettings = configuration.GetSection(nameof(AuthenticationSettings));
            services.Configure<AuthenticationSettings>(authenticationSettings);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AuthenticationSettings>>().Value);

            return authenticationSettings.Get<AuthenticationSettings>();
        }

    }
}
