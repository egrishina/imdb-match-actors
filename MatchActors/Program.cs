using MatchActors.Infrastructure.Database;
using MatchActors.Infrastructure.ImdbClient;
using MediatR;

namespace MatchActors
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
            builder.Services.Configure<ImdbOptions>(builder.Configuration.GetSection(nameof(ImdbOptions)));
            builder.Services.AddMediatR(typeof(Program));
            builder.Services.AddSingleton<ICommandBuilder, CommandBuilder>();
            builder.Services.AddSingleton<IActorsRepository, ActorsRepository>();
            builder.Services.AddHttpClient<IImdbClient, ImdbClient>();
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}