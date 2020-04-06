using System.Security.Claims;

namespace WorldDomination.DelegatedAuthentication.Application.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Name(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue("Name");
        }

        public static string Email(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        }

        public static string Picture(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue("Picture");
        }
    }
}
