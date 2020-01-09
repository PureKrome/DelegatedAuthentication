using System.Threading;

namespace WorldDomination.DelegatedAuthentication.Tests.AuthenticationServiceTests
{
    public class NoOpAuthenticationOptions : ICreateANewAccountOrGetAnExistingAccountOptions
    {
        public object SomeDatabaseContext { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
