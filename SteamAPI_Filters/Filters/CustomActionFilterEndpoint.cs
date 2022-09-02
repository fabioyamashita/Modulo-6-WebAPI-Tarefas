using Microsoft.AspNetCore.Mvc.Filters;

namespace SteamAPI.Filters
{
    public class CustomActionFilterEndpoint : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Executado depois da chamada do método (OnActionExecuted)");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Executado antes da chamada do método (OnActionExecuting)");
        }
    }
}
