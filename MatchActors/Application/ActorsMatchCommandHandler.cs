using Dapper;
using MediatR;
using Newtonsoft.Json;
using Npgsql;

namespace MatchActors.Application;

internal sealed class ActorsMatchCommandHandler : IRequestHandler<ActorsMatchCommand, IEnumerable<ActorsMatchResult>>
{
    public async Task<IEnumerable<ActorsMatchResult>> Handle(ActorsMatchCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<ActorsMatchResult>();

        // TODO
        var key = "k_7nutarhc";

        var cs = @"Server=127.0.0.1:5432;Database=actorsdb;User ID=postgres;Password=qwerty";

        using var con = new NpgsqlConnection(cs);

        con.Open();
        var q1 = "select actor_id from actors where name='" + request.Actor1 + "';";
        var q2 = "select actor_id from actors where name='" + request.Actor2 + "';";

        var val1 = con.ExecuteScalar<string>(q1);
        var val2 = con.ExecuteScalar<string>(q2);

        var clnt = new HttpClient();

        if (val1 == null)
        {
            var c = await clnt.GetAsync("https://imdb-api.com/en/API/SearchName/" + key + "/" + request.Actor1);
            var res = await c.Content.ReadAsStringAsync();
            var a = JsonConvert.DeserializeObject<Data>(res);

            val1 = a.Results.FirstOrDefault(t => request.Actor1 == t.Title)?.Id;
        }

        if (val2 == null)
        {
            var c = await clnt.GetAsync("https://imdb-api.com/en/API/SearchName/" + key + "/" + request.Actor2);
            var res = await c.Content.ReadAsStringAsync();
            var a = JsonConvert.DeserializeObject<Data>(res);

            val2 = a.Results.FirstOrDefault(t => request.Actor2 == t.Title)?.Id;
        }

        if (val1 != null && val2 != null)
        {
            var m1 = await clnt.GetAsync("https://imdb-api.com/en/API/Name/" + key + "/" + val1);
            var res1 = await m1.Content.ReadAsStringAsync();
            var movs1 = JsonConvert.DeserializeObject<ActorData>(res1).CastMovies;

            var m2 = await clnt.GetAsync("https://imdb-api.com/en/API/Name/" + key + "/" + val2);
            var res2 = await m2.Content.ReadAsStringAsync();
            var movs2 = JsonConvert.DeserializeObject<ActorData>(res2).CastMovies;

            // filter MoviesOnly
            //if (request.MoviesOnly == true)
            //{
            //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
            //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
            //}

            foreach (var movies1 in movs1)
            {
                foreach (var movies2 in movs2)
                {
                    if (movies1.Id == movies2.Id)
                    {
                        result.Add(new ActorsMatchResult { Movie = movies1.Title });
                    }
                }
            }
        }

        return result;
    }

    public class Data
    {
        public Actor[] Results { get; set; }
    }

    public class Actor
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }

    public class ActorData
    {
        public Movie[] CastMovies { get; set; }
    }

    public class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
    }
}