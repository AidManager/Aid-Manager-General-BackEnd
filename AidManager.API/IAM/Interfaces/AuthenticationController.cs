using System.Net.Mime;
using AidManager.API.IAM.Domain.Services;
using AidManager.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using AidManager.API.IAM.Interfaces.REST.Resources;
using AidManager.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AidManager.API.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]

public class AuthenticationController(IUserIAMCommandService userCommandService) : ControllerBase
{
    
    [HttpPost("sign-in")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "SignIn",
        Description = "Signs in the user and returns the token by validating the Email and Password.",
        OperationId = "SignIn"
    )]
    public async Task<IActionResult> SignIn([FromBody] SignInResource resource)
    {
        try
        {
            var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var authenticatedUser = await userCommandService.Handle(signInCommand);
            var authenticatedUserResource =
                AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                    authenticatedUser.token);
            return Ok(authenticatedUserResource);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
        
    }
}