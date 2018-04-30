using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorldDomination.DelegatedAuthentication
{
    /// <inheritdoc />
    public class AuthenticationService<TSourceJwt, TCustomJwt, TOptions, TAccount>
        : IAuthenticationService<TSourceJwt, TCustomJwt, TOptions, TAccount>
        where TSourceJwt : Jwt, new()
        where TCustomJwt : Jwt, new()
        where TOptions : ICreateANewAccountOrGetAnExistingAccountOptions, new()
        where TAccount : new()
    {
        private readonly string _customJwtSecret;
        private readonly string _sourceJwtSecret;

        public AuthenticationService(string sourceJwtSecret,
                                     string customJwtSecret)
        {
            if (string.IsNullOrWhiteSpace(sourceJwtSecret))
            {
                throw new ArgumentException(nameof(sourceJwtSecret));
            }

            if (string.IsNullOrWhiteSpace(customJwtSecret))
            {
                throw new ArgumentException(nameof(customJwtSecret));
            }

            _sourceJwtSecret = sourceJwtSecret;
            _customJwtSecret = customJwtSecret;
        }

        /// <inheritdoc />
        public bool IsJwtExpiryValidatedWhenDecoding { private get; set; } = true;

        /// <inheritdoc />
        public async Task<AuthenticationResult<TAccount>> AuthenticateAsync(string bearerToken,
                                                                            TOptions createNewAccountOrGetExistingAccountOptions,
                                                                            Func<TSourceJwt, TOptions, CancellationToken, Task<TAccount>> createNewAccountOrGetExistingAccount,
                                                                            Func<TAccount, TSourceJwt, TCustomJwt> copyAccountToCustomJwt,
                                                                            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                throw new ArgumentNullException(nameof(bearerToken));
            }

            if (createNewAccountOrGetExistingAccount == null)
            {
                throw new ArgumentNullException(nameof(createNewAccountOrGetExistingAccount));
            }

            if (copyAccountToCustomJwt == null)
            {
                throw new ArgumentNullException(nameof(copyAccountToCustomJwt));
            }

            // First, lets decode the source token to make sure it's legit.
            var sourceJwt = JwtExtensions.Decode<TSourceJwt>(bearerToken,
                                                             _sourceJwtSecret,
                                                             IsJwtExpiryValidatedWhenDecoding);
            if (sourceJwt == null)
            {
                // We failed to decode the bearer token.
                // 1. It's just not in the correct format
                // -or-
                // 2. Failed verification (3x checks).
                return null;
            }

            // Create a new account if they don't already exist in the Db.
            // Otherwise, return the existing account.
            var account = await createNewAccountOrGetExistingAccount(sourceJwt, 
                                                                     createNewAccountOrGetExistingAccountOptions,
                                                                     cancellationToken);
            if (account == null)
            {
                return null;
            }

            // Left-to-right copying.
            var customJwt = copyAccountToCustomJwt(account, sourceJwt);
            if (customJwt == null)
            {
                return null;
            }

            // Finally, Encode this new account into a new token.
            var customBearerToken = customJwt.Encode(_customJwtSecret);

            return new AuthenticationResult<TAccount>(customBearerToken, account);
        }
    }
}