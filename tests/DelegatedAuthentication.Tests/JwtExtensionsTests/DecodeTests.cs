using System;
using Homely.Testing;
using Shouldly;
using WorldDomination.DelegatedAuthentication.Tests.AuthenticationServiceTests;
using Xunit;

namespace WorldDomination.DelegatedAuthentication.Tests.JwtExtensionsTests
{
    public class DecodeTests
    {
        public static TheoryData<string, string> BadData
        {
            get
            {
                const string clientSecret = "some secret";

                return new TheoryData<string, string>
                {
                    {
                        "",
                        clientSecret
                    },
                    {
                        "aaa",
                        clientSecret
                    },

                    // Expiry is in 1970, so it shouldn't pass validation.
                    {
                        new CustomJwt().Encode(clientSecret),
                        clientSecret
                    }
                };
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GivenABearerToken_Decode_ReturnsACustomJwt(bool verify)
        {
            // Arrange.
            var jwt = new CustomJwt
            {
                Sub = "Somename",
                UserName = "Leia",
                Exp = verify
                    ? DateTime.UtcNow.AddDays(1).Ticks // Should pass verification
                    : 0 // Normally would fail verification
            };
            const string clientSecret = "some secret";
            var token = jwt.Encode(clientSecret);

            // Act.
            var result = JwtExtensions.Decode<CustomJwt>(token, clientSecret, verify);

            // Assert.
            result.ShouldLookLike(jwt);
        }

        [Theory]
        [MemberData(nameof(BadData))]
        public void GivenABadBearerToken_Decode_ReturnsNull(string token, string secret)
        {
            // Arrange & Act.
            var result = JwtExtensions.Decode<CustomJwt>(token, secret, true);

            // Assert.
            result.ShouldBeNull();
        }
    }
}
