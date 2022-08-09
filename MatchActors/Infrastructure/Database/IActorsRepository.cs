namespace MatchActors.Infrastructure.Database;

internal interface IActorsRepository
{
    Task<ActorInfo> GetActorInfo(string actorName, CancellationToken cancellationToken);
}