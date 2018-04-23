using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldDomination.DelegatedAuthentication.Auth0;
using WorldDomination.DelegatedAuthentication.WebApi.Models;
using WorldDomination.DelegatedAuthentication.WebApi.Services;

namespace WorldDomination.DelegatedAuthentication.WebApi.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ApplicationSettings _applicationSettings;
        private readonly IAuthenticationService<Auth0Jwt, CustomJwt, CustomAuthenticationOptions, Account> _authenticationService;

        public AuthenticationController(IAuthenticationService<Auth0Jwt, CustomJwt, CustomAuthenticationOptions, Account> authenticationService,
                                        IAccountService accountService,
                                        ApplicationSettings applicationSettings)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult> Authenticate(string bearerToken,
                                                     CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return BadRequest();
            }

            var authenticationOptions = new CustomAuthenticationOptions
            {
                SomeDatabaseContext = new object(), // This would be were you would pass in the current EF Context or RavenDb session, etc.
            };

            var newBearerToken = await _authenticationService.AuthenticateAsync(bearerToken,
                                                                                authenticationOptions,
                                                                                CreateNewAccountOrGetExistingAccount,
                                                                                CopyAccountToCustomJwt,
                                                                                cancellationToken);
            if (string.IsNullOrWhiteSpace(newBearerToken))
            {
                // We failed to decode and/or create a new bearerToken.
                return BadRequest("IdToken is invalid, fails to pass validation, has expired.");
            }

            return Ok(new
            {
                bearerToken = newBearerToken
            });
        }

        [Authorize]
        [HttpGet("AuthenticateTest")]
        public ActionResult AuthenticateTest()
        {
            var claims = User.Claims.Select(claim => new
                             {
                                 claim.Type,
                                 claim.Value
                             })
                             .ToArray();
            var result = new
            {
                Name = User.Name(),
                Email = User.Email(),
                Picture = User.Picture(),
                Claims = claims
            };
            return Ok(result);
        }

        private Task<Account> CreateNewAccountOrGetExistingAccount(Auth0Jwt sourceJwt, 
                                                                   CustomAuthenticationOptions options,
                                                                   CancellationToken cancellationToken)
        {
            if (options == null)
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
            var existingAccount = _accountService.GetOrCreateAccount(account, options.SomeDatabaseContext, cancellationToken);

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