using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.IAM.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AidManager.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
        {
            Console.WriteLine("All good");
            return;
        }
        // If user is logged in, this will be set
        var user = (UserAuth?)context.HttpContext.Items["UserAuth"];

        if (user == null) context.Result = new UnauthorizedResult();
    }
}