using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Interfaces.REST.Resources;

namespace AidManager.API.Collaborate.Interfaces.REST.Transform;

public class UpdatePostCommandFromResource
{
    public static UpdatePostCommand FromResourceToCommand(int id, int authorId, int companyId ,UpdatePostResource resource)
    {
        return new UpdatePostCommand(
            id,
            resource.Title,
            resource.Subject,
            resource.Description,
            companyId,
            authorId,
            resource.Images
        );
    }
}