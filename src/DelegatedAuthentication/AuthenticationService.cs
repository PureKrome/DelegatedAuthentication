using System;
using System.Threading;

namespace WorldDomination.DelegatedAuthentication
{
    /// <inheritdoc />
    public class AuthenticationService<TSourceJwt, TCustomJwt, TDatabaseContext, TUser> : IAuthenticationService<TSourceJwt, TCustomJwt, TDatabaseContext, TUser>
        where TSourceJwt : Jwt, new()
        where TCustomJwt : Jwt, new()
        where TDatabaseContext : new()
        where TUser : new()
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
        public string Authenticate(string bearerToken,
                                   Func<TSourceJwt, TDatabaseContext, CancellationToken, TUser> createNewAccountOrGetExistingAccount,
                                   Func<TUser, TSourceJwt, TCustomJwt> copyAccountToCustomJwt,
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

            // Create a new account if they don't already exist in our Db.
            var account = createNewAccountOrGetExistingAccount(sourceJwt, cancellationToken);
            if (account == null)
            {
                throw new Exception(
                    "Expected an 'account' to be retrieved (either new or an existing one) but recieved an 'null' account. We need to have an account created at this point, to continue.");
            }

            // Left-to-right copying.
            var customJwt = copyAccountToCustomJwt(account, sourceJwt);

            // Finally, Encode this new account into a new token.
            return customJwt.Encode(_customJwtSecret);
        }
    }
}