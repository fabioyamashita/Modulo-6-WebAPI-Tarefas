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
        private int _id;
        private Games _gamePreviousState { get; set; } = new Games();

        public CustomActionFilterLogger(IBaseRepository<Games> repository)
        {
            _repository = repository;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //if (body.GetType().Equals(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult)))
            //{
            //    var res = ((Microsoft.AspNetCore.Mvc.OkObjectResult)context.Result).Value;

            //    if (res.GetType().Equals(typeof(Games)))
            //    {
            //        Games response = res as Games;

            //        Console.WriteLine($"{DateTime.Now.ToString("G")} - Game {response.Id} ");
            //    }
            //}

            if (context.HttpContext.Request.Method == "PUT" || context.HttpContext.Request.Method == "PATCH")
            {
                var res = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;

                Games response = res as Games;

                string message = $"{DateTime.Now.ToString("G")} - Game {response.Id} - {response.Name} - Atualizado de:" +
                    $"{JsonSerializer.Serialize(_gamePreviousState)} para {JsonSerializer.Serialize(response)}";

                Console.WriteLine(message);
            }

            else if (context.HttpContext.Request.Method == "DELETE")
            {
                string message = $"{DateTime.Now.ToString("G")} - Game {_gamePreviousState.Id} - {_gamePreviousState.Name} - Removido";

                Console.WriteLine(message);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method == "PUT" ||
                context.HttpContext.Request.Method == "PATCH" ||
                context.HttpContext.Request.Method == "DELETE")
            {
                _id = int.Parse(context.ActionArguments["id"].ToString());

                var _gamePreviousStateCopy = _repository.GetByKey(_id).Result;

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

        // Fazer o console.write no console
        public void LogInformation()
        {

        }

        // Salvar em um arquivo txt
        // Criar um novo LogRespository
        public void SaveLogToFile()
        {

        }
    }
}
