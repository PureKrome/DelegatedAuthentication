using Shouldly;
using WorldDomination.DelegatedAuthentication.Tests.AuthenticationServiceTests;
using Xunit;

namespace WorldDomination.DelegatedAuthentication.Tests.JwtExtensionsTests
{
    public class EncodeTests
    {
        [Fact]
        public void GivenAJwt_Encode_ReturnsABearerToken()
        {
            // Arrange.
            var jwt = new CustomJwt
            {
                Sub = "Somename",
                UserName = "Leia"
            };
            const string clientSecret = "some secret";

            // Act.
            var token = jwt.Encode(clientSecret);

            // Assert.
            token.ShouldNotBeNullOrWhiteSpace();
        }
    }
}
