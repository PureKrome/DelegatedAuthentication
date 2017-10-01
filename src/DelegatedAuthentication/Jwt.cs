namespace WorldDomination.DelegatedAuthentication
{
    public class Jwt
    {
        /// <summary>
        /// Issuer.
        /// </summary>
        public string Iss { get; set; }

        /// <summary>
        /// Subject.
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// Audience.
        /// </summary>
        public string Aud { get; set; }

        /// <summary>
        /// Expiration time.
        /// </summary>
        public long Exp { get; set; }

        /// <summary>
        /// Issued At.
        /// </summary>
        public long Iat { get; set; }
    }
}