using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WorldDomination.DelegatedAuthentication.Application.Authentication.Queries
{
    public class GetAuthenticateTestQuery : IRequest<GetAuthenticateTestQuery.Response>
    {
        public GetAuthenticateTestQuery(ClaimsPrincipal claimsPrincipal)
        {
            User = claimsPrincipal ?? throw new ArgumentNullException(nameof(claimsPrincipal));
        }

        public ClaimsPrincipal User { get; }

        public class Response
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Picture { get; set; }
            public IEnumerable<Claim> Claims { get; set; } = Enumerable.Empty<Claim>();
        }

        public class GetAuthenticateTestQueryHandler : IRequestHandler<GetAuthenticateTestQuery, Response>
        {
            public Task<Response> Handle(GetAuthenticateTestQuery request, CancellationToken cancellationToken)
            {
                var claims = request.User.Claims.Select(claim => new Claim(claim.Type, claim.Value)).ToList();

                var result = new Response
                {
                    Name = request.User.Name(),
                    Email = request.User.Email(),
                    Picture = request.User.Picture(),
                    Claims = claims
                };

                return Task.FromResult(result);
            }
        }
    }
}
