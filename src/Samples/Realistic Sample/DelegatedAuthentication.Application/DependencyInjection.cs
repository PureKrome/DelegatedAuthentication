using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorldDomination.DelegatedAuthentication.Application.Accounts;
using WorldDomination.DelegatedAuthentication.Application.Authentication;
using WorldDomination.DelegatedAuthentication.Application.Common.Interfaces;
using WorldDomination.DelegatedAuthentication.Application.Common.Settings;
using WorldDomination.DelegatedAuthentication.Application.Settings;

namespace WorldDomination.DelegatedAuthentication.Application
{
    public static class DependencyInjection
    {
        public static AuthenticationSettings AddApplication(this IServiceCollection services,
                                                            IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new System.ArgumentNullException(nameof(configuration));
            }

            var settings = services.AddApplicationSettings(configuration);

            services.AddCustomAuthentication(settings);

            services.AddSingleton<IAccountService, AccountService>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            return settings;
        }
    }
}
