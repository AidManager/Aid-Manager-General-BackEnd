using System.Threading.Tasks;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;

namespace AidManager.API.Collaborate.Domain.Services;

public interface IPostCommandService
{
    Task<(Post?,User)> Handle(CreatePostCommand command);
    Task<(Post post, User user, List<(Comments?, User)> comments)> Handle(DeletePostCommand command);
    
    
    Task<(Post post, User user, List<(Comments?, User)> comments)> Handle(UpdatePostRatingCommand command);
    
    
    Task<PostImage?> Handle(DeletePostImageCommand command);
    
    Task<(Post?,User,List<(Comments?, User)>)> Handle(UpdatePostCommand command);
    
}