using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SteamAPI.Filters
{
    public class V1DiscontinuedResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // Caso não queira deletar a controllerV1
            // Realizando um Short-circuit para descontinuar a V1 da API
            // Mandar uma mensagem falando para usar a V2

            if (context.HttpContext.Request.Path.Value.ToLower().Contains("v1"))
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Success = false,
                    Errors = new[] { "A v1 foi descontinuada, por favor, use a v2." }
                });
            }
        }
    }
}
