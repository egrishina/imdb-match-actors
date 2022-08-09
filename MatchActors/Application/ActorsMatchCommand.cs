using MediatR;

namespace MatchActors.Application;

internal sealed class ActorsMatchCommand : IRequest<IEnumerable<ActorsMatchResult>>
{
    public string Actor1 { get; init; } = string.Empty;
    
    public string Actor2 { get; init; } = string.Empty;
    
    public bool MoviesOnly { get; init; }
}