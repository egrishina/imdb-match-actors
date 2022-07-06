using Dapper;

namespace MatchActors.Infrastructure.Database;

internal interface ICommandBuilder
{
    public CommandDefinition BuildCommand(string actorName, CancellationToken cancellationToken);
}