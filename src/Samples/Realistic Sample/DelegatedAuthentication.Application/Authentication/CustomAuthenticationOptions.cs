using System.Threading;

namespace WorldDomination.DelegatedAuthentication.Application.Authentication
{
    public class CustomAuthenticationOptions : ICreateANewAccountOrGetAnExistingAccountOptions
    {
        public object SomeDatabaseContext { get; set; }
    }
}
