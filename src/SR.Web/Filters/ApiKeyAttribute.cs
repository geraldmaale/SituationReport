using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SR.Web.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "ApiKey";
        
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
        {
            // context.Result = new UnauthorizedResult();
            context.Result = new ContentResult()
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Content = "Api key was not provided or wrong"
            };
            return;
        }

        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var apikey = configuration.GetValue<string>(ApiKeyHeaderName);

        if (!apikey.Equals(potentialApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Content = "Api key was not provided or wrong"
            };
            return;
        }

        await next();

    }
}