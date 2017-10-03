using System.Threading;
using WorldDomination.DelegatedAuthentication.WebApi.Models;

namespace WorldDomination.DelegatedAuthentication.WebApi.Services
{
    public interface IAccountService
    {
        Account GetOrCreateAccount(Account account, object dbContextOrSession, CancellationToken cancellationToken);
    }
}