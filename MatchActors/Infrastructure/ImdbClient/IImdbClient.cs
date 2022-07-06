namespace MatchActors.Infrastructure.ImdbClient;

internal interface IImdbClient
{
    public Task<string> GetActorId(string actorName, CancellationToken cancellationToken);

    public Task<List<Movie>> GetActorMovies(string actorId, CancellationToken cancellationToken);
}