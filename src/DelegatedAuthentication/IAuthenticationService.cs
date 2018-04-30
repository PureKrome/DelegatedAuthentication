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
    /// <typeparam name="TOptions">ICreateANewAccountOrGetAnExistingAccountOptions: some custom options that are needed for you to help check/store this account into your db.</typeparam>
    /// <typeparam name="TAccount">Your account.</typeparam>
    public interface IAuthenticationService<out TSourceJwt, in TCustomJwt, TOptions, TAccount>
        where TSourceJwt : Jwt, new()
        where TCustomJwt : Jwt, new()
        where TOptions : ICreateANewAccountOrGetAnExistingAccountOptions, new()
        where TAccount : new()
    {
        /// <summary>
        /// Do we validate the original JWT when we first receive it and decode it?
        /// </summary>
        /// <remarks>You should <i>only</i> set this to <code>false</code> if you are testing.</remarks>
        bool IsJwtExpiryValidatedWhenDecoding { set; }

        /// <summary>
        /// This authenticates the provided 3rd party bearer Token with our own system.
        /// If the Account doesn't exist, then we should create a new Account. Otherwise, use the exisiting Account.
        /// </summary>
        /// <param name="bearerToken">string: the source JWT bearer token from the 3rd party Authentication Service.</param>
        /// <param name="createANewAccountOrGetAnExistingAccountOptions">Method options for you custom <code>createNewAccountOrGetExistingAccount</code> func.</param>
        /// <param name="createNewAccountOrGetExistingAccount">Func: a method that will create a new Account or use an existing Account, then return that Account.</param>
        /// <param name="copyAccountToCustomJwt">Func: a method which will end up copying over any Account information into your custom JWT.</param>
        /// <param name="cancellationToken">Cancellation token to cancel any running execution.</param>
        /// <returns>AuthenticationResult: a result that incldudes your new custom JWT bearer token and the authenticated Account or <code>null</code> if it failed to authenticate.</returns>
        Task<AuthenticationResult<TAccount>> AuthenticateAsync(string bearerToken,
                                                            TOptions createANewAccountOrGetAnExistingAccountOptions,
                                                            Func<TSourceJwt, TOptions, CancellationToken, Task<TAccount>> createNewAccountOrGetExistingAccount,
                                                            Func<TAccount, TSourceJwt, TCustomJwt> copyAccountToCustomJwt,
                                                            CancellationToken cancellationToken = default(CancellationToken));
    }
}