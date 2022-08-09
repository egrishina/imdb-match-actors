using MediatR;

namespace MatchActors.Application;

internal sealed class ActorsMatchCommandHandler : IRequestHandler<ActorsMatchCommand, IEnumerable<ActorsMatchResult>>
{
    private readonly ICachedActorsResolver _actorsResolver;
    private readonly IActorsMatcher _actorsMatcher;
    private readonly IMediator _mediator;

    public ActorsMatchCommandHandler(ICachedActorsResolver actorsResolver, IActorsMatcher actorsMatcher,
        IMediator mediator)
    {
        _actorsResolver = actorsResolver;
        _actorsMatcher = actorsMatcher;
        _mediator = mediator;
    }

    public async Task<IEnumerable<ActorsMatchResult>> Handle(ActorsMatchCommand request,
        CancellationToken cancellationToken)
    {
        var actorId1 = await _actorsResolver.ResolveActorId(request.Actor1, cancellationToken);
        var actorId2 = await _actorsResolver.ResolveActorId(request.Actor2, cancellationToken);

        var commonMovies =
            await _actorsMatcher.GetCommonMovies(actorId1, actorId2, request.MoviesOnly, cancellationToken);

        var actorsMatchResult = commonMovies.Select(movie =>
            new ActorsMatchResult { Movie = movie }).ToList();

        await _mediator.Publish(new ActorsMatchNotification
        {
            Command = request,
            Result = actorsMatchResult
        }, cancellationToken);

        return actorsMatchResult;
    }
}