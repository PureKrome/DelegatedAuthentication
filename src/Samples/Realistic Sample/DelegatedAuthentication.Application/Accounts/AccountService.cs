using System;
using System.Collections.Concurrent;
using System.Threading;
using WorldDomination.DelegatedAuthentication.Application.Common.Interfaces;
using WorldDomination.DelegatedAuthentication.Domain;

namespace WorldDomination.DelegatedAuthentication.Application.Accounts
{
    public class AccountService : IAccountService
    {
        // Fake 'database' of accounts. Only good for this demo because it exists only in RAM.
        private readonly ConcurrentDictionary<string, Account> _accounts;

        // Internal account Id "identity" counter. Only good for this demo, etc.
        private int _accountId;

        public AccountService(ConcurrentDictionary<string, Account> accounts)
        {
            _accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
        }

        /// <inheritdoc/>
        public Account GetOrCreateAccount(Account account, object dbContextOrSession)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (dbContextOrSession == null)
            {
                throw new ArgumentNullException(nameof(dbContextOrSession));
            }

            // NOTE: We're using an inmemory account (because we're cheating in this Sample project), 
            //       so we don't need to use the dbContext and/or cancellation token. 
            var newOrExistingAccount = _accounts.GetOrAdd(account.Email, account);
            if (newOrExistingAccount.Id == 0)
            {
                newOrExistingAccount.Id = Interlocked.Increment(ref _accountId);
            }

            return newOrExistingAccount;
        }
    }
}
