using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SteamAPI.Interfaces;
using SteamAPI.Logs;
using SteamAPI.Models;
using System.Text.Json;

namespace SteamAPI.Filters
{
    public class CustomActionFilterLogger : Attribute, IActionFilter
    {
        private readonly IBaseRepository<Games> _repository;
        private readonly ILogRepository _logRepository;
        private readonly List<int> _successStatusCodes;

        private Games _gamePreviousState { get; set; } = new Games();

        public CustomActionFilterLogger(IBaseRepository<Games> repository, ILogRepository logRepository)
        {
            _repository = repository;
            _logRepository = logRepository;
            _successStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status201Created };
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Path.Value.StartsWith("/api/Games/", StringComparison.InvariantCultureIgnoreCase) &&
                _successStatusCodes.Contains(context.HttpContext.Response.StatusCode))
            {
                var id = int.Parse(context.HttpContext.Request.Path.ToString().Split("/").Last());

                if (ContextContainsRequestMethods(context, "put", "patch"))
                {
                    var gameCurrentState = _repository.GetByKey(id).Result;

                    CustomLogs.SaveLog(_logRepository, _gamePreviousState, gameCurrentState);
                }

                else if (ContextContainsRequestMethods(context, "delete"))
                {
                    CustomLogs.SaveLog(_logRepository, _gamePreviousState);
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (String.Equals(context.ActionDescriptor.RouteValues["controller"], "games", StringComparison.InvariantCultureIgnoreCase) &&
                ContextContainsRequestMethods(context, "put", "patch", "delete"))
            {
                var id = int.Parse(context.ActionArguments["id"].ToString());

                var _gamePreviousStateCopy = _repository.GetByKey(id).Result;

                if (_gamePreviousStateCopy != null)
                {
                    // Criando um objeto cópia (Shallow Copy) usando MemberwiseClone
                    // Como a minha classe Games não possui nenhuma propriedade do tipo referência,
                    // não tem problema, pois as variáveis do tipo valor são copiadas.
                    // Se existisse algum tipo referência, só o endereço na memória seria copiado
                    _gamePreviousState = (Games)_gamePreviousStateCopy.Clone();
                }
            }
        }

        private static bool ContextContainsRequestMethods(FilterContext context, params string[] methods)
        {
            if (methods.Any(method => context.HttpContext.Request.Method.Equals(method, StringComparison.InvariantCultureIgnoreCase)))
                return true;

            return false;
        }
    }
}
