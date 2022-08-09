using Dapper;

namespace MatchActors.Infrastructure.Database;

internal sealed class CommandBuilder : ICommandBuilder
{
    private const string BaseQuery = "select actor_id as Id, name as Title from actors where name='@actor'";
    
    public CommandDefinition BuildCommand(string actorName, CancellationToken cancellationToken)
    {
        return new CommandDefinition(BaseQuery, new { actor = actorName }, cancellationToken: cancellationToken);
    }
}