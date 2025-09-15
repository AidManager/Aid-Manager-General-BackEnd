using System.Net.Mime;
using AidManager.API.Authentication.Interfaces.REST.Transform;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Collaborate.Interfaces.REST.Resources;
using AidManager.API.Collaborate.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AidManager.API.Collaborate.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PostsController(IPostCommandService postCommandService, IPostQueryService postQueryService, IFavoritePostQueryService favoritePostQueryService) : ControllerBase
{
    // create new post
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new post",
        Description = "Create a new post",
        OperationId = "CreatePost"
    )]
    [SwaggerResponse(201, "Post created", typeof(CreatePostResource))]
    public async Task<IActionResult> CreateNewPost([FromBody] CreatePostResource resource)
    {
        try
        {
            var command = CreatePostCommandFromResourceAssembler.ToCommandFromResource(resource);
            var post = await postCommandService.Handle(command);

            if (post.Item1 == null) return BadRequest();

            var postResource = PostResourceFromEntityAssembler.ToResourceFromEntity(post.Item1, post.Item2, []);
            return Ok(postResource);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
        
    }

    [HttpPut("{id}/update/{userId}/company/{companyId}")]
    [SwaggerOperation(
        Summary = "Update post by id",
        Description = "Update post by id",
        OperationId = "UpdatePost")]
    [SwaggerResponse(200, "The post was updated", typeof(UpdatePostResource))]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, int userId, int companyId, [FromBody] UpdatePostResource resource)
    {
        try
        {
            var command = UpdatePostCommandFromResource.FromResourceToCommand(id, userId, companyId, resource);
            var post = await postCommandService.Handle(command);

            if (post.Item1 == null) return NotFound();

            var comment = post.Item3;
            
            var commentResources = comment.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList();

            var postResource = PostResourceFromEntityAssembler.ToResourceFromEntity(post.Item1, post.Item2, commentResources);
            
            return Ok(postResource);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
    }
    
    
    
     // update rating field of post by id
        [HttpPatch("{id}/rating/{userId}")]
        [SwaggerOperation(
            Summary = "Update rating field of post by id",
            Description = "Update rating field of post by id",
            OperationId = "UpdatePostRating"
        )]
        [SwaggerResponse(200, "The post rating was updated: ", typeof(CreatePostResource))]
        public async Task<IActionResult> UpdatePostRating([FromRoute] int id, int userId)
        {
            try
            {
                var command = UpdatePostRatingCommandFromResourceAssembler.ToCommandFromResource(id, userId);
                var post = await postCommandService.Handle(command);
    
                var comment = post.comments;
            
                var commentResources = comment.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList();

                var postResource = PostResourceFromEntityAssembler.ToResourceFromEntity(post.Item1, post.Item2, commentResources);

                return Ok(postResource);
            }
            catch (Exception e)
            {
                return BadRequest("Error: " + e.Message);
            }
            
        }
        
    // obtain post by id
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get post by id",
        Description = "Get post by id",
        OperationId = "GetPostById"
    )]
    [SwaggerResponse(200, "The post was found", typeof(PostResource))]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        try
        {
            var query = new GetPostById(id);
            var post = await postQueryService.Handle(query);

            var comment = post.comments;
            
            var commentResources = comment.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList();

            var postResource = PostResourceFromEntityAssembler.ToResourceFromEntity(post.Item1, post.Item2, commentResources);

            return Ok(postResource);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
        
    }
    
    // delete post by id
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete post by id",
        Description = "Delete post by id",
        OperationId = "DeletePostById"
    )]
    [SwaggerResponse(200, "The post was deleted", typeof(CreatePostResource))]
    public async Task<IActionResult> DeletePostById([FromRoute] int id)
    {
        try
        {
            var command = new DeletePostCommand(id);
            var post = await postCommandService.Handle(command);

            if (post.Item1 == null) return NotFound();

            var comment = post.comments;
            
            var commentResources = comment.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList();

            var postResource = PostResourceFromEntityAssembler.ToResourceFromEntity(post.Item1, post.Item2, commentResources);
            
            return Ok(postResource);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
        
    }
    
    // get all posts by company id
    [HttpGet("company/{companyId}")]
    [SwaggerOperation(
        Summary = "Get all posts by company id",
        Description = "Get all posts by company id",
        OperationId = "GetAllPostsByCompanyId"
    )]
    [SwaggerResponse(200, "The posts by company id were found", typeof(CreatePostResource))]
    public async Task<IActionResult> GetAllPostsByCompanyId([FromRoute] int companyId)
    {
        try
        {
            var query = new GetAllPostsByCompanyId(companyId);
            var posts = await postQueryService.Handle(query);

            var comment = posts.Select(tuple=>tuple.Item3);
            
            var commentResources = comment.Select(tuple=>tuple.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList()).ToList();

            var postResources = posts.Select(tuple =>
            {
                if (tuple.Item1 != null)
                    return PostResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2,
                        commentResources.ElementAt(posts.IndexOf(tuple)));
                return null;
            });
            return Ok(postResources);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
        
    }
    [HttpGet("user/{userId}")]
    [SwaggerOperation(
        Summary = "Get all posts by author/user id",
        Description = "Get all posts by user id",
        OperationId = "GetAllPostsByUserId"
    )] 
    public async Task<IActionResult> GetAllPostsByUserId([FromRoute] int userId)
    {
        try
        {
            var query = new GetPostByAuthor(userId);
            var posts = await postQueryService.Handle(query);

            if (posts == null) return NotFound();

            var comment = posts.Select(tuple=>tuple.Item3);
            
            var commentResources = comment.Select(tuple=>tuple.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList()).ToList();

            var postResources = posts.Select(tuple =>
            {
                if (tuple.Item1 != null)
                    return PostResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2,
                        commentResources.ElementAt(posts.IndexOf(tuple)));
                return null;
            });            return Ok(postResources);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
        
    }
    //show liked posts
    
    [HttpGet("liked/{userId}")]
    [SwaggerOperation(
        Summary = "Get all liked posts by user id",
        Description = "Get all liked posts by user id",
        OperationId = "GetLikedPostsByUserId"
    )]
    public async Task<IActionResult> GetLikedPostsByUserId([FromRoute] int userId)
    {
        try
        {
            var query = new GetLikedPostsByUserid(userId);
            var posts = await postQueryService.Handle(query);

            if (posts == null) return NotFound();

            var comment = posts.Select(tuple=>tuple.Item3);
            
            var commentResources = comment.Select(tuple=>tuple.Select(t=>CommentResourceFromEntityAssembler.ToResourceFromEntity(t.Item1,t.Item2)).ToList()).ToList();

            var postResources = posts.Select(tuple =>
            {
                if (tuple.Item1 != null)
                    return PostResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2,
                        commentResources.ElementAt(posts.IndexOf(tuple)));
                return null;
            });            return Ok(postResources);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
        
    }
    
   
}