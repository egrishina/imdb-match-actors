namespace MatchActors.Infrastructure.ImdbClient;

internal sealed class ImdbOptions
{
    public string BaseUrl { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
    public string SearchName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}