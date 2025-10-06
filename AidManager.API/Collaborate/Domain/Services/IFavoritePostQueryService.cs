using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;

namespace AidManager.API.Collaborate.Domain.Services;

public interface IFavoritePostQueryService
{
    Task<List<(Post, User, List<(Comments?, User)>)>> Handle(GetFavoritePosts query);
    
}