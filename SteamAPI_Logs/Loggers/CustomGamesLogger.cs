using SteamAPI.Models;
using System.Text.Json;

namespace SteamAPI.Loggers
{
    public class CustomGamesLogger : ILogger
    {
        public DateTime? Date { get; set; }
        public Games? GamesPreviousState { get; set; }
        public Games? GamesCurrentState { get; set; }


        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }

        public void LogUpdateInformation(Games gamesPreviousState, Games gamesCurrentState)
        {
            Console.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - " +
                $"Game {gamesCurrentState.Id} - {gamesCurrentState.Name} - " +
                $"Alterado de {JsonSerializer.Serialize(gamesPreviousState)} para {JsonSerializer.Serialize(gamesCurrentState)}");
        }

        public void LogDeleteInformation(Games gamesPreviousState)
        {
            Console.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - " +
                $"Game {gamesPreviousState.Id} - {gamesPreviousState.Name} - Removido");
        }
    }
}
