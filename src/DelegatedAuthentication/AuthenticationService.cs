using System;

namespace WorldDomination.DelegatedAuthentication
{
    public class AuthenticationService<TSourceJwt, TCustomJwt> : IAuthenticationService<TSourceJwt, TCustomJwt>
        where TSourceJwt : Jwt, new()
        where TCustomJwt : Jwt, new()
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
        public string Authenticate(string bearerToken,
                                   Func<TSourceJwt, object> createNewAccountOrGetExistingAccount,
                                   Func<object, TSourceJwt, TCustomJwt> copyAccountToCustomJwt)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                throw new ArgumentNullException(nameof(bearerToken));
            }

            // First, lets decode the source token.
            var sourceJwt = JwtExtensions.Decode<TSourceJwt>(bearerToken,
                                                             _sourceJwtSecret,
                                                             IsJwtExpiryValidatedWhenDecoding);
            if (sourceJwt == null)
            {
                // We failed to decode the bearer token.
                return null;
            }

            // Create a new account if they don't already exist in our Db.
            var account = createNewAccountOrGetExistingAccount(sourceJwt);
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

        /// <inheritdoc />
        public bool IsJwtExpiryValidatedWhenDecoding { private get; set; } = true;
    }
}