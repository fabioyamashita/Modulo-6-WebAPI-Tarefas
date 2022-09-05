using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SteamAPI.Interfaces;
using SteamAPI.Models;
using System.Text.Json;

namespace SteamAPI.Filters
{
    public class CustomActionFilterLogger : Attribute, IActionFilter
    {
        private readonly IBaseRepository<Games> _repository;
        private readonly ILogRepository _logRepository;

        private Games _gamePreviousState { get; set; } = new Games();

        public CustomActionFilterLogger(IBaseRepository<Games> repository, ILogRepository logRepository)
        {
            _repository = repository;
            _logRepository = logRepository;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Method == "PUT" || context.HttpContext.Request.Method == "PATCH")
            {
                var res = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;

                Games response = res as Games;

                SaveLog(_gamePreviousState, response);
            }

            else if (context.HttpContext.Request.Method == "DELETE")
            {
                SaveLog(_gamePreviousState);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method == "PUT" ||
                context.HttpContext.Request.Method == "PATCH" ||
                context.HttpContext.Request.Method == "DELETE")
            {
                var id = int.Parse(context.ActionArguments["id"].ToString());

                var _gamePreviousStateCopy = _repository.GetByKey(id).Result;

                // Criando um objeto cópia
                // se eu atribuisse simplesmente com =, ele seria um ponteiro
                // Sendo assim, uma vez modificado ou deletado o original, ele também mudaria
                _gamePreviousState.Id = _gamePreviousStateCopy.Id;
                _gamePreviousState.AppId = _gamePreviousStateCopy.AppId;
                _gamePreviousState.Name = _gamePreviousStateCopy.Name;
                _gamePreviousState.Developer = _gamePreviousStateCopy.Developer;
                _gamePreviousState.Platforms = _gamePreviousStateCopy.Platforms;
                _gamePreviousState.Categories = _gamePreviousStateCopy.Categories;
                _gamePreviousState.Genres = _gamePreviousStateCopy.Genres;
            }
        }

        // Update (Put/Patch)
        private void SaveLog(Games gamesPreviousState, Games gamesCurrentState)
        {
            string message = $"{DateTime.Now.ToString("G")} - Game {gamesCurrentState.Id} - {gamesCurrentState.Name} " +
                             $"- Atualizado de:" +
                             $"{JsonSerializer.Serialize(gamesPreviousState)} " +
                             $"para {JsonSerializer.Serialize(gamesCurrentState)}";

            Console.WriteLine(message);

            _logRepository.Insert(message);
        }

        // Delete
        private void SaveLog(Games gamesPreviousState)
        {
            string message = $"{DateTime.Now.ToString("G")} - Game {gamesPreviousState.Id} - {gamesPreviousState.Name} - Removido";

            Console.WriteLine(message);

            _logRepository.Insert(message);
        }
    }
}
