using MatchActors.Application.Exceptions;
using MatchActors.Infrastructure.Database;
using MatchActors.Infrastructure.ImdbClient;

namespace MatchActors.Application;

internal sealed class CachedActorsResolver : ICachedActorsResolver
{
    private readonly IActorsRepository _actorsRepository;
    private readonly IImdbClient _imdbClient;

    public CachedActorsResolver(IActorsRepository actorsRepository, IImdbClient imdbClient)
    {
        _actorsRepository = actorsRepository;
        _imdbClient = imdbClient;
    }

    public async Task<string> ResolveActorId(string actorName, CancellationToken cancellationToken)
    {
        var resultFromDb = await _actorsRepository.GetActorInfo(actorName, cancellationToken);
        if (resultFromDb is null)
        {
            var resultFromImdbClient = await _imdbClient.GetActorId(actorName, cancellationToken);
            if (resultFromImdbClient is null)
            {
                throw new ActorNotFoundException($"Actor {actorName} was not found");
            }

            return resultFromImdbClient;
        }

        return resultFromDb.Id;
    }
}