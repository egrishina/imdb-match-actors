using MatchActors.Infrastructure.Database;
using MatchActors.Infrastructure.ImdbClient;
using MediatR;

namespace MatchActors.Application;

internal sealed class ActorsMatchCommandHandler : IRequestHandler<ActorsMatchCommand, IEnumerable<ActorsMatchResult>>
{
    private readonly IActorsRepository _actorsRepository;
    private readonly IImdbClient _imdbClient;

    public ActorsMatchCommandHandler(IActorsRepository actorsRepository, IImdbClient imdbClient, IMediator mediator)
    {
        _actorsRepository = actorsRepository;
        _imdbClient = imdbClient;
    }
    
    public async Task<IEnumerable<ActorsMatchResult>> Handle(ActorsMatchCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<ActorsMatchResult>();

        var actorInfo1 = await _actorsRepository.GetActorInfo(request.Actor1, cancellationToken);
        var actorInfo2 = await _actorsRepository.GetActorInfo(request.Actor2, cancellationToken);

        var id1 = actorInfo1?.Id;
        var id2 = actorInfo2?.Id;

        if (id1 == null)
        {
            id1 = await _imdbClient.GetActorId(request.Actor1, cancellationToken);
        }

        if (id2 == null)
        {
            id2 = await _imdbClient.GetActorId(request.Actor2, cancellationToken);
        }

        if (id1 != null && id2 != null)
        {
            var movs1 = await _imdbClient.GetActorMovies(id1, cancellationToken);
            var movs2 = await _imdbClient.GetActorMovies(id2, cancellationToken);

            // filter MoviesOnly
            //if (request.MoviesOnly == true)
            //{
            //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
            //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
            //}

            foreach (var movies1 in movs1)
            {
                foreach (var movies2 in movs2)
                {
                    if (movies1.Id == movies2.Id)
                    {
                        result.Add(new ActorsMatchResult { Movie = movies1.Title });
                    }
                }
            }
        }

        return result;
    }
}