using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using WorldDomination.DelegatedAuthentication.Domain;

namespace DelegatedAuthentication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var accountsRepository = new ConcurrentDictionary<string, Account>();
            services.AddSingleton(accountsRepository);

            return services;
        }
    }
}
