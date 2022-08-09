using MediatR;

namespace MatchActors.Application;

internal sealed class ActorsMatchNotification : INotification
{
    public ActorsMatchCommand Command { get; init; } = new();

    public List<ActorsMatchResult> Result { get; init; } = new();
}