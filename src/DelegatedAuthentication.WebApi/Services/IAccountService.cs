using WorldDomination.DelegatedAuthentication.WebApi.Models;

namespace WorldDomination.DelegatedAuthentication.WebApi.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// If the account exists, just return it.
        /// If the account does NOT exist, create it and then return it.
        /// </summary>
        /// <param name="account">Account to check if it exists.</param>
        /// <param name="dbContextOrSession">Some type of DB context.</param>
        /// <returns>An account.</returns>
        Account GetOrCreateAccount(Account account, object dbContextOrSession);
    }
}
