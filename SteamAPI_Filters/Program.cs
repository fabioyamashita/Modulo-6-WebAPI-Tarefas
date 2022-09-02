using Microsoft.EntityFrameworkCore;
using SteamAPI.Context;
using SteamAPI.Filters;
using SteamAPI.Interfaces;
using SteamAPI.Repositories;

namespace SteamAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionando logging nas dependências
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddControllers(options => {
                options.Filters.Add(typeof(CustomActionFilterGlobal));
                options.Filters.Add(typeof(V1DiscontinuedResourceFilter));
                //options.Filters.Add(typeof(CustomExceptionFilterGlobal));
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Conexao In Memory database

            builder.Services.AddDbContext<InMemoryContext>(options => options.UseInMemoryDatabase("Steam"));

            #endregion

            #region Injecao de dependencia do Repository
            
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            
            #endregion

            #region Registra o Data Generator
            
            builder.Services.AddTransient<DataGenerator>();
            
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.Logger.LogInformation("Configuring https...");
            app.UseHttpsRedirection();

            app.Logger.LogInformation("Configuring authorization...");
            app.UseAuthorization();

            app.Logger.LogInformation("Configuring MapControllers...");
            app.MapControllers();

            #region Popula o banco de dados
            app.Logger.LogInformation("Creating database...");
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<DataGenerator>();
                service.Generate();
            }
            #endregion

            app.Logger.LogInformation("Starting the app...");
            app.Run();
        }
    }
}