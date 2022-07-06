namespace MatchActors.Infrastructure.ImdbClient;

internal sealed class ImdbActorMoviesDto
{
    public List<Movie> CastMovies { get; init; } = new();
}

internal sealed class Movie
{
    public string Id { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
}