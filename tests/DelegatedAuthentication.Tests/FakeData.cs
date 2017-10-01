using System;
using WorldDomination.DelegatedAuthentication.Auth0;
using WorldDomination.DelegatedAuthentication.Tests.AuthenticationServiceTests;

namespace WorldDomination.DelegatedAuthentication.Tests
{
    public static class FakeData
    {
        public static Auth0Jwt FakeAuth0Jwt => new Auth0Jwt
        {
            Iss = "https://<your-auth0-tenant>.au.auth0.com/",
            Sub = "google-oauth2|11671232005123412341234",
            Aud = "MYsa7s3Uw75IfNj7H123412341234s",
            Exp = 1496361742,
            Iat = 1496325742,
            Name = "Leia Organa",
            Email = "i.am.a.princess@rebelalliance.net",
            Picture = "http://images.techtimes.com/data/images/full/168082/princess-leia-organa.jpg?w=600",
            Gender = "female",
            Locale = "en-au"
        };

        public static CustomJwt FakeCustomJwt(Auth0Jwt auth0Jwt,
                                              FakeAccount account)
        {
            if (auth0Jwt == null)
            {
                throw new ArgumentNullException(nameof(auth0Jwt));
            }

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            return new CustomJwt
            {
                Iss = auth0Jwt.Iss,
                Sub = auth0Jwt.Sub,
                Aud = auth0Jwt.Aud,
                Exp = auth0Jwt.Exp,
                Iat = auth0Jwt.Iat,
                Id = account.Id,
                FullName = account.Name
            };
        }
    }
}