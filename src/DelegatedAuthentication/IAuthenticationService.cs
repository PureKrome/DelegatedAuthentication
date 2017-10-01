using System;

namespace WorldDomination.DelegatedAuthentication
{
    public interface IAuthenticationService<out TSourceJwt, in TCustomJwt>
        where TSourceJwt : Jwt, new()
        where TCustomJwt : Jwt, new()
    {
        /// <summary>
        /// This authenticates the provided bearer Token with our own system.
        /// If the User doesn't exist, then we should create a new user. Otherwise, use the exisiting User.
        /// </summary>
        /// <param name="bearerToken">string: the source JWT bearer token from the 3rd party Authentication Service.</param>
        /// <param name="createNewAccountOrGetExistingAccount">func: a method that will create a new User or use an existing User. It should return a custom User account.</param>
        /// <param name="copyAccountToCustomJwt">func: a method which will end up copying over any User information into your custom JWT.</param>
        /// <returns>string: a new custom JWT.</returns>
        string Authenticate(string bearerToken,
                            Func<TSourceJwt, object> createNewAccountOrGetExistingAccount,
                            Func<object, TSourceJwt, TCustomJwt> copyAccountToCustomJwt);

        /// <summary>
        /// Do we validate the original JWT when we first receive it and decode it?
        /// </summary>
        /// <remarks>You should <i>only</i> set this to <code>false</code> if you are testing.</remarks>
        bool IsJwtExpiryValidatedWhenDecoding { set; }
    }
}