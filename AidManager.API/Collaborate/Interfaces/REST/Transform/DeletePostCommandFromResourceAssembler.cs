using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Interfaces.REST.Transform;

public class DeletePostCommandFromResourceAssembler
{
    public static DeletePostCommand ToCommandFromResource(DeletePostResource resource)
    {
        return new DeletePostCommand(resource.Id);
    }
}