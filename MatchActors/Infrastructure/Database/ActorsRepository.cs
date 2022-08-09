using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace MatchActors.Infrastructure.Database;

internal sealed class ActorsRepository : IActorsRepository
{
    private readonly ICommandBuilder _commandBuilder;
    private readonly ConnectionStrings _connectionStrings;

    public ActorsRepository(ICommandBuilder commandBuilder, IOptions<ConnectionStrings> connectionStrings)
    {
        _commandBuilder = commandBuilder;
        _connectionStrings = connectionStrings.Value;
    }

    public async Task<ActorInfo> GetActorInfo(string actorName, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionStrings.PostgresConnectionString);
        await connection.OpenAsync(cancellationToken);
        var command = _commandBuilder.BuildCommand(actorName, cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<ActorInfo>(command);
    }
}