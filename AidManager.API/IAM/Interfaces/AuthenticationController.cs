using System.Net.Mime;
using AidManager.API.IAM.Domain.Services;
using AidManager.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using AidManager.API.IAM.Interfaces.REST.Resources;
using AidManager.API.IAM.Interfaces.REST.Transform;
using AidManager.API.IAM.Interfaces.Security;              
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AidManager.API.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthenticationController(
    IUserIAMCommandService userCommandService,
    IReCaptchaValidator captcha                                  
) : ControllerBase
{
    [HttpPost("sign-in")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "SignIn",
        Description = "Signs in the user and returns the token by validating the Email and Password.",
        OperationId = "SignIn"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SignIn([FromBody] SignInResource resource, CancellationToken ct)
    {
        try
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var token = resource.CaptchaToken
                       ?? Request.Form["g-recaptcha-response"].ToString();

            if (string.IsNullOrWhiteSpace(token))
                return BadRequest(new { message = "captchaToken requerido" });

            var captchaOk = await captcha.ValidateAsync(token, remoteIp, ct);
            if (!captchaOk)
                return Forbid(); 

            var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var authenticatedUser = await userCommandService.Handle(signInCommand);
            var authenticatedUserResource =
                AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(
                    authenticatedUser.user, authenticatedUser.token);

            return Ok(authenticatedUserResource);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
    }
}
