using AidManager.API.Collaborate.Domain.Model.Commands;

namespace AidManager.API.Collaborate.Domain.Model.Aggregates;

public class LikedPosts
{
    public int UserId { get; set; }
    public int PostId { get; set; }
    
    public LikedPosts()
    {
        
    }
    public LikedPosts(UpdatePostRatingCommand command)
    {
        UserId = command.UserId;
        PostId = command.PostId;
    }
}