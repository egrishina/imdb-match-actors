namespace MatchActors.Dtos;

/// <summary>
/// Response to match actors
/// </summary>
public sealed class ActorsMatchResponse
{
    /// <summary>
    /// Movie title with matched actors
    /// </summary>
    public string Movie { get; init; } = string.Empty;
}