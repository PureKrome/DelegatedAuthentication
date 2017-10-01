using System;
using System.Collections.Concurrent;
using System.Threading;
using WorldDomination.DelegatedAuthentication.WebApi.Models;

namespace WorldDomination.DelegatedAuthentication.WebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly ConcurrentDictionary<string, Account> _accounts;
        private int _accountId;

        public AccountService(ConcurrentDictionary<string, Account> accounts)
        {
            _accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
        }

        public Account GetOrCreateAccount(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            var isAdded = _accounts.TryAdd(account.Email, account);
            if (isAdded)
            {
                account.Id = Interlocked.Increment(ref _accountId);
            }

            return account;
        }
    }
}