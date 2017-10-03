using System.Threading.Tasks;
using Shouldly;
using WorldDomination.DelegatedAuthentication.Auth0;
using Xunit;

namespace WorldDomination.DelegatedAuthentication.Tests.AuthenticationServiceTests
{
    public class AuthenticationAsyncTests
    {
        [Fact]
        public async Task GivenAnInvalidBearerToken_AuthenticateAsync_ReturnsNull()
        {
            // Arrange.
            const string sourceJwtSecret = "pewpewpew";
            const string customJwtSecret = "adsadas";
            const string bearerToken = "aaaaaaaaaaa";

            var authenticationOptions = new CustomAuthenticationOptions();
            Task<FakeAccount> CreateNewAccountOrGetExistingAccount(Auth0Jwt sourceJwt, CustomAuthenticationOptions options) => null;
            CustomJwt CopyAccountToCustomJwt(FakeAccount a, Auth0Jwt sourceJwt) => FakeData.FakeCustomJwt(null, null);

            var authenticationService = new AuthenticationService<Auth0Jwt, CustomJwt, CustomAuthenticationOptions, FakeAccount>(sourceJwtSecret, customJwtSecret)
            {
                IsJwtExpiryValidatedWhenDecoding = false
            };

            // Act.
            var token = await authenticationService.AuthenticateAsync(bearerToken,
                                                           authenticationOptions, 
                                                           CreateNewAccountOrGetExistingAccount,
                                                           CopyAccountToCustomJwt);

            // Assert.
            token.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GivenAValidBearerToken_Authenticate_CreatesAnAccountAndReturnsANewToken()
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
            var authenticationOptions = new CustomAuthenticationOptions();
            var authenticationService = new AuthenticationService<Auth0Jwt, CustomJwt, CustomAuthenticationOptions, FakeAccount>(sourceJwtSecret, customJwtSecret)
            {
                IsJwtExpiryValidatedWhenDecoding = false
            };

            Task<FakeAccount> CreateNewAccountOrGetExistingAccount(Auth0Jwt sourceJwt, CustomAuthenticationOptions options) => Task.FromResult(account);
            CustomJwt CopyAccountToCustomJwt(FakeAccount a, Auth0Jwt sourceJwt) => FakeData.FakeCustomJwt(sourceJwt, a);

            // Act.
            var token = await authenticationService.AuthenticateAsync(bearerToken,
                                                           authenticationOptions,
                                                           CreateNewAccountOrGetExistingAccount,
                                                           CopyAccountToCustomJwt);

            // Assert.
            token.ShouldNotBeNullOrWhiteSpace();
        }
    }
}