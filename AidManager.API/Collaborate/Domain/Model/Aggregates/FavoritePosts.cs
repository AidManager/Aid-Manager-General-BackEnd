using AidManager.API.Collaborate.Domain.Model.Commands;

namespace AidManager.API.Collaborate.Domain.Model.ValueObjects;

public class FavoritePosts
{
    public int UserId { get; set; }
    public int PostId { get; set; }
    
    public FavoritePosts()
    {
        
    }
    public FavoritePosts(FavoritePostCommand command)
    {
        UserId = command.UserId;
        PostId = command.PostId;
    }
    
    
    
    
    
}

