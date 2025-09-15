using AidManager.API.Authentication.Domain.Services;
using AidManager.API.IAM.Application.Internal.OutboundServices;
using AidManager.API.IAM.Domain.Model.Queries;
using AidManager.API.IAM.Domain.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AidManager.API.IAM.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IUserIAMQueryService userQueryService,
        ITokenService tokenService
    )
    {
        try
        {
            Console.WriteLine("Entering InvokeAsync");
            var path = context.Request.Path.Value;
            var allowAnonymous = path.Equals("/api/v1/authentication/sign-up", StringComparison.OrdinalIgnoreCase) ||
                                 path.Equals("/api/v1/authentication/sign-in", StringComparison.OrdinalIgnoreCase) ||
                                 path.Equals("/api/v1/users/sign-up", StringComparison.OrdinalIgnoreCase);
            

            Console.WriteLine($"Allow Anonymous is {allowAnonymous}");
            if (allowAnonymous)
            {
                Console.WriteLine("Skipping Authorization");
                await next(context);
                return;
            }

            Console.WriteLine("Checking Authorization");
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?
                .Split(" ").Last();
            if (token == null) throw new Exception("Null or invalid token");
            var userId = await tokenService.ValidateToken(token);
            if (userId == null) throw new Exception("Invalid token");
            var getUserByIdQuery = new GetUserIAMByIdQuery(userId.Value);
            var user = await userQueryService.Handle(getUserByIdQuery);
            if (user == null) throw new Exception("User not found");
            Console.WriteLine("Authorization successful");
            context.Items["UserAuth"] = user;
            Console.WriteLine("Continuing with Middleware pipeline");
            await next(context);
        }
        catch (Exception e)
        {
            // return a bad request unauthorized result
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(e.Message);
        }
    }
}
