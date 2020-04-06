using System;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace WorldDomination.DelegatedAuthentication
{
    public static class JwtExtensions
    {
        private static readonly IJwtAlgorithm Algorithm = new HMACSHA256Algorithm();
        private static readonly IDateTimeProvider Provider = new UtcDateTimeProvider();
        private static readonly IJsonSerializer Serializer = new JsonNetSerializer(new CustomJsonSerializer());
        private static readonly IBase64UrlEncoder UrlEncoder = new JwtBase64UrlEncoder();
        private static readonly IJwtValidator Validator = new JwtValidator(Serializer, Provider);
        private static readonly IJwtDecoder Decoder = new JwtDecoder(Serializer, Validator, UrlEncoder, Algorithm);
        private static readonly IJwtEncoder Encoder = new JwtEncoder(Algorithm, Serializer, UrlEncoder);

        public static string Encode(this Jwt payload,
                                    string clientSecret)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (clientSecret == null)
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            return Encoder.Encode(payload, clientSecret);
        }

        public static T Decode<T>(string token,
                                  string clientSecret,
                                  bool verify = true)
        {
            try
            {
                return Decoder.DecodeToObject<T>(token, clientSecret, verify);
            }
            catch
            {
                // Validation failed (i.e. expired) -or- token wasn't a valid JWT.
                return default;
            }
        }
    }
}
