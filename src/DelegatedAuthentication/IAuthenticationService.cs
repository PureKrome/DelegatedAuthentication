using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorldDomination.DelegatedAuthentication
{
    /// <summary>
    /// Interface that defines how we can authenticate to your own system given a authenticated 3rd party JWT.
    /// </summary>
    /// <typeparam name="TSourceJwt">Jwt: 3rd party JWT from some service which we have delegated the authentication, too.</typeparam>
    /// <typeparam name="TCustomJwt">Jwt: you own custom JWT, based off the Source JWT.</typeparam>
    /// <typeparam name="TOptions">ICreateANewAccountOrGetAnExistingAccountOptions: some custom options that are needed for you to help check/store this user into your db.</typeparam>
    /// <typeparam name="TUser">Your user/account.</typeparam>
    public interface IAuthenticationService<out TSourceJwt, in TCustomJwt, TOptions, TUser>
        where TSourceJwt : Jwt, new()
        where TCustomJwt : Jwt, new()
        where TOptions : ICreateANewAccountOrGetAnExistingAccountOptions, new()
        where TUser : new()
    {
        /// <summary>
        /// Do we validate the original JWT when we first receive it and decode it?
        /// </summary>
        /// <remarks>You should <i>only</i> set this to <code>false</code> if you are testing.</remarks>
        bool IsJwtExpiryValidatedWhenDecoding { set; }

        /// <summary>
        /// This authenticates the provided bearer Token with our own system.
        /// If the User doesn't exist, then we should create a new user. Otherwise, use the exisiting User.
        /// </summary>
        /// <param name="bearerToken">string: the source JWT bearer token from the 3rd party Authentication Service.</param>
        /// <param name="createANewAccountOrGetAnExistingAccountOptions">Method options for you custom <code>createNewAccountOrGetExistingAccount</code> func.</param>
        /// <param name="createNewAccountOrGetExistingAccount">func: a method that will create a new User or use an existing User. It should return a custom User account.</param>
        /// <param name="copyAccountToCustomJwt">func: a method which will end up copying over any User information into your custom JWT.</param>
        /// <param name="cancellationToken">Cancellation token to cancel any running execution.</param>
        /// <returns>string: a new custom JWT.</returns>
        Task<string> AuthenticateAsync(string bearerToken,
                                       TOptions createANewAccountOrGetAnExistingAccountOptions,
                                       Func<TSourceJwt, TOptions, CancellationToken, Task<TUser>> createNewAccountOrGetExistingAccount,
                                       Func<TUser, TSourceJwt, TCustomJwt> copyAccountToCustomJwt,
                                       CancellationToken cancellationToken = default(CancellationToken));
    }
}