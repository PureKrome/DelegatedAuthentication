using System.Threading;

namespace WorldDomination.DelegatedAuthentication.WebApi.Models
{
    public class CustomAuthenticationOptions : ICreateANewAccountOrGetAnExistingAccountOptions
    {
        public object SomeDatabaseContext { get; set; }
    }
}