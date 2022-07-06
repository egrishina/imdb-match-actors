using MatchActors.Application;
using MatchActors.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MatchActors.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorsMatchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActorsMatchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ActorsMatchResponse>>> Post([FromBody] ActorsMatchRequest request,
            CancellationToken cancellationToken)
        {
            var actorsMatchResults = await _mediator.Send(new ActorsMatchCommand
            {
                Actor1 = request.Actor1,
                Actor2 = request.Actor2,
                MoviesOnly = request.MoviesOnly
            }, cancellationToken);

            var response = actorsMatchResults.Select(result =>
                new ActorsMatchResponse
                {
                    Movie = result.Movie
                });

            return new ActionResult<IEnumerable<ActorsMatchResponse>>(response);
        }
    }
}