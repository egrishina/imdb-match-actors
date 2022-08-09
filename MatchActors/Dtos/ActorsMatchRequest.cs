namespace MatchActors.Dtos;

/// <summary>
/// Request to match actors
/// </summary>
public sealed class ActorsMatchRequest
{
    /// <summary>
    /// First actor's name
    /// </summary>
    public string Actor1 { get; init; } = string.Empty;
    
    /// <summary>
    /// Second actor's name
    /// </summary>
    public string Actor2 { get; init; } = string.Empty;
    
    /// <summary>
    /// Flag to search for matches only in the movies
    /// </summary>
    public bool MoviesOnly { get; init; }
}