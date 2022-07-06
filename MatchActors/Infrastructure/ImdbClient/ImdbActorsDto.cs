namespace MatchActors.Infrastructure.ImdbClient;

internal sealed class ImdbActorsDto
{
    public List<Actor> Results { get; init; } = new();
}

internal sealed class Actor
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
}