namespace MatchActors.Application;

public interface ICachedActorsResolver
{
    public Task<string> ResolveActorId(string actorName, CancellationToken cancellationToken);
}