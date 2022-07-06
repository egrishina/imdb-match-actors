using Microsoft.Extensions.Options;

namespace MatchActors.Infrastructure.ImdbClient;

internal sealed class ImdbClient : IImdbClient
{
    private readonly HttpClient _httpClient;
    private readonly ImdbOptions _imdbOptions;

    public ImdbClient(HttpClient httpClient, IOptions<ImdbOptions> imdbOptions)
    {
        _httpClient = httpClient;
        _imdbOptions = imdbOptions.Value;
    }
    
    public async Task<string> GetActorId(string actorName, CancellationToken cancellationToken)
    {
        var imdbResponse = await _httpClient.GetFromJsonAsync<ImdbActorsDto>(BuildSearchNameQuery(actorName), cancellationToken);
        return imdbResponse?.Results.FirstOrDefault(x => x.Title == actorName)?.Id;
    }

    public async Task<List<Movie>> GetActorMovies(string actorId, CancellationToken cancellationToken)
    {
        var imdbResponse = await _httpClient.GetFromJsonAsync<ImdbActorMoviesDto>(BuildNameQuery(actorId), cancellationToken);
        return imdbResponse?.CastMovies;
    }

    private string BuildSearchNameQuery(string actorName)
    {
        return $"{_imdbOptions.BaseUrl}/{_imdbOptions.SearchName}/{_imdbOptions.ApiKey}/{actorName}";
    }

    private string BuildNameQuery(string actorId)
    {
        return $"{_imdbOptions.BaseUrl}/{_imdbOptions.Name}/{_imdbOptions.ApiKey}/{actorId}";
    }
}