using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
namespace ApiKey
{
    public class AuthFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var apiKeyFromHeaders))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var apikey = configuration.GetValue<string>("ApiKey");

            if (!apikey.Equals(apiKeyFromHeaders))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}