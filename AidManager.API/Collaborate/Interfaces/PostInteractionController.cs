
using System.Net.Mime;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Collaborate.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AidManager.API.Collaborate.Interfaces;

[ApiController]
[Route("api/v1/posts/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PostInteractionController(IFavoritePostCommandService favoritePostCommandService, IFavoritePostQueryService favoritePostQueryService, IPostQueryService postQueryService) : ControllerBase
{
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Add Favorite Post",
        Description = "Add a post to favorite posts",
        OperationId = "AddFavoritePost"
    )]
    public async Task<IActionResult> FavoritePost( [FromBody] FavoritePostCommand command)
    {
        try
        {
            var result = await favoritePostCommandService.Handle(command);

            if (result == false)
            {
                return BadRequest("Post already in favorites");
            }
            
            var post = await postQueryService.Handle(new GetPostById(command.PostId));
            var comment = post.comments;
            
            var commentResources = comment.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList();

            var resource = PostResourceFromEntityAssembler.ToResourceFromEntity(post.Item1, post.Item2, commentResources);

            return Ok(resource);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
    }
    
    [HttpDelete]
    [SwaggerOperation(
        Summary = "Remove favorite post",
        Description = "Remove favorite post",
        OperationId = "RemoveFavoritePost"
    )]
    public async Task<IActionResult> RemoveFavoritePost( [FromBody] RemoveFavoritePostCommand command)
    {
        try
        {
            var result = await favoritePostCommandService.Handle(command);

            if (result == false)
            {
                return BadRequest("Post already deleted from favorites");
            }
            
            var post = await postQueryService.Handle(new GetPostById(command.PostId));
            if (post.Item1 == null)
            {
                return BadRequest("Post not found");

            }                
            var comment = post.comments;
            
            var commentResources = comment.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList();

            var resource = PostResourceFromEntityAssembler.ToResourceFromEntity(post.Item1, post.Item2, commentResources);
            return Ok(resource);

        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
    }
    
    [HttpGet("user/{userId}")]
    [SwaggerOperation(
        Summary = "Get all favorite posts by user id",
        Description = "Get all favorite posts by user id",
        OperationId = "GetFavoritePostsByUserId"
    )]
    public async Task<IActionResult> GetFavoritePosts([FromRoute] int userId)
    {
        try
        {
            var posts = await favoritePostQueryService.Handle(new GetFavoritePosts(userId));
            
            var comment = posts.Select(tuple=>tuple.Item3);
            
            var commentResources = comment.Select(tuple=>tuple.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList()).ToList();

            var postResources = posts.Select(tuple =>
            { 
                return PostResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2,
                    commentResources.ElementAt(posts.IndexOf(tuple)));
               
            });
            return Ok(postResources);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
    }
    
    
    
}