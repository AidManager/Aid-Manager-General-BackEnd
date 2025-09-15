using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Interfaces.REST.Transform;

public static class CreatePostCommandFromResourceAssembler
{
    public static CreatePostCommand ToCommandFromResource(CreatePostResource resource)
    {
        return new CreatePostCommand(
            resource.Title, 
            resource.Subject,
            resource.Description, 
            resource.CompanyId,
            resource.UserId,
            resource.Images
        );
    }    
}