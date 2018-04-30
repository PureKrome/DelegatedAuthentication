namespace WorldDomination.DelegatedAuthentication
{
    /// <summary>
    /// Simple Account information now that the 3rd party Bearer Token has been successfully converted to your Bearer Token.
    /// </summary>
    public class AuthenticationResult<T>
    {
        public AuthenticationResult(string bearerToken,
                                    T account)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                throw new System.ArgumentException(nameof(bearerToken));
            }

            BearerToken = bearerToken;
            Account = account;
        }

        /// <summary>
        /// New bearer token with your own account claims and schema. 
        /// </summary>
        public string BearerToken { get; }

        /// <summary>
        /// The Account whom was either created OR already existed.
        /// </summary>
        public T Account { get; }
    }
}
