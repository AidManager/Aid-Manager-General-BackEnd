using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Interfaces.REST.Transform;

public static class CreateAddCommentCommandFromResourceAssembler
{
    public static AddCommentCommand ToCommandFromResource(int projectId,AddCommentResource resource)
    {
        return new AddCommentCommand(resource.UserId, resource.Comment, projectId);
    }
}