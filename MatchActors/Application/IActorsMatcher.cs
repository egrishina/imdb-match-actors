namespace MatchActors.Application;

internal interface IActorsMatcher
{
    public Task<List<string>> GetCommonMovies(string actorId1, string actorId2, bool moviesOnly,
        CancellationToken cancellationToken);
}