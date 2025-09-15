using AidManager.API.Collaborate.Domain.Model.Commands;

namespace AidManager.API.Collaborate.Domain.Model.ValueObjects;

public class Comments
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Comment { get; set; }
    public int PostId { get; set; }
    public DateTime TimeOfComment { get; set; } = DateTime.Now;
    
    public Comments() {}
    public Comments(AddCommentCommand command)
    {
        UserId = command.UserId;
        Comment = command.Comment;
        PostId = command.PostId;
        TimeOfComment = DateTime.Now;
    }
    
}