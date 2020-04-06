using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldDomination.DelegatedAuthentication.Application.Authentication.Commands.CreateAuthenticate;
using WorldDomination.DelegatedAuthentication.Application.Authentication.Queries;

namespace WorldDomination.DelegatedAuthentication.RealisticWebApi.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // POST Authenticate
        [HttpPost("Authenticate")]
        public async Task<ActionResult<CreateAuthenticateCommand.Response>> Authenticate(CreateAuthenticateCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [Authorize]
        [HttpGet("AuthenticateTest")]
        public async Task<ActionResult<GetAuthenticateTestQuery.Response>> AuthenticateTest(CancellationToken cancellationToken)
        {
            var query = new GetAuthenticateTestQuery(User);
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
