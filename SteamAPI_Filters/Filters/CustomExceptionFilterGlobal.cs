using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace SteamAPI.Filters
{
    // Não cobre um HttpResponseException 
    public class CustomExceptionFilterGlobal : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(new
            {
                message = "Ops! Ocorreu um erro inesperado!"
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
