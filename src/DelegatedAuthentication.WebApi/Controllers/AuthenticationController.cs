using System;
using System.Linq;
using WorldDomination.DelegatedAuthentication.Auth0;
using WorldDomination.DelegatedAuthentication.WebApi.Models;
using WorldDomination.DelegatedAuthentication.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorldDomination.DelegatedAuthentication.WebApi.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ApplicationSettings _applicationSettings;
        private readonly IAuthenticationService<Auth0Jwt, CustomJwt> _authenticationService;

        public AuthenticationController(IAuthenticationService<Auth0Jwt, CustomJwt> authenticationService,
                                        IAccountService accountService,
                                        ApplicationSettings applicationSettings)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
        }

        [HttpPost("Authenticate")]
        public ActionResult Authenticate(string bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return BadRequest();
            }

            var newBearerToken = _authenticationService.Authenticate(bearerToken,
                                                                     CreateNewAccountOrGetExistingAccount,
                                                                     CopyAccountToCustomJwt);
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

        private object CreateNewAccountOrGetExistingAccount(Auth0Jwt auth0Jwt)
        {
            if (auth0Jwt == null)
            {
                throw new ArgumentNullException(nameof(auth0Jwt));
            }

            var account = new Account
            {
                Email = auth0Jwt.Email,
                UserName = auth0Jwt.Sub,
                FullName = auth0Jwt.Name
            };

            return _accountService.GetOrCreateAccount(account);
        }

        private CustomJwt CopyAccountToCustomJwt(object o,
                                                 Auth0Jwt auth0Jwt)
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }

            if (auth0Jwt == null)
            {
                throw new ArgumentNullException(nameof(auth0Jwt));
            }

            if (!(o is Account account))
            {
                return null;
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