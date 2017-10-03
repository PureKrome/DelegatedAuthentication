using System.Threading;

namespace WorldDomination.DelegatedAuthentication.WebApi.Models
{
    public class CustomAuthenticationOptions : IAuthenticationOptions
    {
        public object SomeDatabaseContext { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}