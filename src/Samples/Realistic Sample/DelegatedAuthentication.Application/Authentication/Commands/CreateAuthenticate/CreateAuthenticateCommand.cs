using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WorldDomination.DelegatedAuthentication.Application.Common.Interfaces;
using WorldDomination.DelegatedAuthentication.Application.Settings;
using WorldDomination.DelegatedAuthentication.Auth0;
using WorldDomination.DelegatedAuthentication.Domain;

namespace WorldDomination.DelegatedAuthentication.Application.Authentication.Commands.CreateAuthenticate
{
    public class CreateAuthenticateCommand : IRequest<CreateAuthenticateCommand.Response>
    {
        public string BearerToken { get; set; }

        public class Response
        {
            public string BearerToken { get; set; }
            public int UserId { get; set; } // This should be in the BearerToken. So this is here for demo purposes.
        }

        public class CreateAuthenticateCommandHandler : IRequestHandler<CreateAuthenticateCommand, CreateAuthenticateCommand.Response>
        {
            private readonly IAccountService _accountService;
            private readonly AuthenticationSettings _applicationSettings;
            private readonly IAuthenticationService<Auth0Jwt, CustomJwt, CustomAuthenticationOptions, Account> _authenticationService;

            public CreateAuthenticateCommandHandler(IAuthenticationService<Auth0Jwt, CustomJwt, CustomAuthenticationOptions, Account> authenticationService,
                                                    IAccountService accountService,
                                                    AuthenticationSettings applicationSettings)
            {
                _authenticationService = authenticationService ?? throw new System.ArgumentNullException(nameof(authenticationService));
                _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
                _applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
            }

            // Given a bearer token, create an account (if none exists) or retrieve the existing account.
            public async Task<Response> Handle(CreateAuthenticateCommand request, CancellationToken cancellationToken)
            {

                var authenticationOptions = new CustomAuthenticationOptions
                {
                    SomeDatabaseContext = new object(), // This would be were you would pass in the current EF Context or RavenDb session, etc.
                    // SomeDatabaseContext = _efcoreContext // EF Core example.
                    // SomeDatabaseContext = _store.OpenAsyncSession() // RavenDb example.
                };

                // ************************************************************
                // This is the where all the magic happens!
                // Given the Auth0 token (or could be any Delegated Authentication Provider) validate
                // then create (or reuse) an Account in your own DB 
                // and finally return a new JWT representing your OWN account.
                var authenticationResult = await _authenticationService.AuthenticateAsync(request.BearerToken, // the Auth0 Bearer Token.
                                                                                          authenticationOptions, // Options to help authenticate with your own DB.
                                                                                          CreateNewAccountOrGetExistingAccount,
                                                                                          CopyAccountToCustomJwt,
                                                                                          cancellationToken);

                var response = new Response
                {
                    BearerToken = authenticationResult.BearerToken,
                    UserId = authenticationResult.Account.Id
                };

                return response;
            }


            private Task<Account> CreateNewAccountOrGetExistingAccount(Auth0Jwt sourceJwt,
                                                                       CustomAuthenticationOptions options,
                                                                       CancellationToken cancellationToken)
            {
                if (options is null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                var account = new Account
                {
                    Email = sourceJwt.Email,
                    UserName = sourceJwt.Sub,
                    FullName = sourceJwt.Name
                };

                // This would most likely be some async method. But we're just doing an in-memory db so this method isn't
                // async in this sample project.
                var existingAccount = _accountService.GetOrCreateAccount(account, options.SomeDatabaseContext);

                return Task.FromResult(existingAccount);
            }

            private CustomJwt CopyAccountToCustomJwt(Account account,
                                                     Auth0Jwt auth0Jwt)
            {
                if (account == null)
                {
                    throw new ArgumentNullException(nameof(account));
                }

                if (auth0Jwt == null)
                {
                    throw new ArgumentNullException(nameof(auth0Jwt));
                }

                return new CustomJwt
                {
                    Iss = _applicationSettings.CustomAuthority,
                    Aud = _applicationSettings.CustomAudience,
                    Sub = auth0Jwt.Sub,
                    Exp = auth0Jwt.Exp,
                    Iat = auth0Jwt.Iat,
                    Id = account.Id,
                    FullName = account.FullName,
                    UserName = account.UserName,
                    Email = account.Email
                };
            }
        }
    }
}
