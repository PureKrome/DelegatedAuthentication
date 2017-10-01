using System.Threading;
using Shouldly;
using WorldDomination.DelegatedAuthentication.Auth0;
using Xunit;

namespace WorldDomination.DelegatedAuthentication.Tests.AuthenticationServiceTests
{
    public class AuthenticationTests
    {
        [Fact]
        public void GivenAnInvalidBearerToken_Authenticate_ReturnsNull()
        {
            // Arrange.
            const string sourceJwtSecret = "pewpewpew";
            const string customJwtSecret = "adsadas";
            const string bearerToken = "aaaaaaaaaaa";
            FakeAccount CreateNewAccountOrGetExistingAccount(Auth0Jwt bt,
                                                             CancellationToken cancellationToken = default(CancellationToken)) => null;
            CustomJwt CopyAccountToCustomJwt(FakeAccount a, Auth0Jwt sourceJwt) => FakeData.FakeCustomJwt(null, null);
            var authenticationService = new AuthenticationService<Auth0Jwt, CustomJwt, FakeAccount>(sourceJwtSecret, customJwtSecret)
            {
                IsJwtExpiryValidatedWhenDecoding = false
            };

            // Act.
            var token = authenticationService.Authenticate(bearerToken,
                                                           CreateNewAccountOrGetExistingAccount,
                                                           CopyAccountToCustomJwt);

            // Assert.
            token.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public void GivenAValidBearerToken_Authenticate_CreatesAnAccountAndReturnsANewToken()
        {
            // Arrange.
            const string sourceJwtSecret = "pewpewpew";
            const string customJwtSecret = "adsadas";
            var auth0Jwt = FakeData.FakeAuth0Jwt;
            var account = new FakeAccount
            {
                Id = 1,
                Name = auth0Jwt.Name
            };
            var bearerToken = auth0Jwt.Encode(sourceJwtSecret);
            var authenticationService = new AuthenticationService<Auth0Jwt, CustomJwt, FakeAccount>(sourceJwtSecret, customJwtSecret)
            {
                IsJwtExpiryValidatedWhenDecoding = false
            };

            FakeAccount CreateNewAccountOrGetExistingAccount(Auth0Jwt bt,
                                                             CancellationToken cancellationToken = default(CancellationToken)) => account;
            CustomJwt CopyAccountToCustomJwt(FakeAccount a, Auth0Jwt sourceJwt) => FakeData.FakeCustomJwt(sourceJwt, a);

            // Act.
            var token = authenticationService.Authenticate(bearerToken,
                                                           CreateNewAccountOrGetExistingAccount,
                                                           CopyAccountToCustomJwt);

            // Assert.
            token.ShouldNotBeNullOrWhiteSpace();
        }
    }
}