using MatchActors.Infrastructure.ImdbClient;

namespace MatchActors.Application;

internal sealed class ActorsMatcher : IActorsMatcher
{
    private readonly IImdbClient _imdbClient;

    public ActorsMatcher(IImdbClient imdbClient)
    {
        _imdbClient = imdbClient;
    }
    
    public async Task<List<string>> GetCommonMovies(string actorId1, string actorId2, bool moviesOnly, CancellationToken cancellationToken)
    {
        var movies1 = await GetActorMovies(actorId1, moviesOnly, cancellationToken);
        var movies2 = await GetActorMovies(actorId2, moviesOnly, cancellationToken);

        var matchResult = new List<string>();
        foreach (var movie1 in movies1)
        {
            foreach (var movie2 in movies2)
            {
                if (movie1.Id == movie2.Id)
                {
                    matchResult.Add(movie1.Title);
                }
            }
        }

        return matchResult;
    }
    
    private async Task<List<Movie>> GetActorMovies(string actorId, bool moviesOnly, CancellationToken cancellationToken)
    {
        var movies = await _imdbClient.GetActorMovies(actorId, cancellationToken);

        if (moviesOnly)
        {
            movies = movies.Where(m => m.Role == "Actress" || m.Role == "Actor").ToList();
        }

        return movies;
    }
}