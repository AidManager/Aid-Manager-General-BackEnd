using AidManager.API.Collaborate.Domain.Model.Commands;

namespace AidManager.API.Collaborate.Domain.Services;

public interface IFavoritePostCommandService
{
    Task<bool> Handle(FavoritePostCommand command);
    Task<bool> Handle(RemoveFavoritePostCommand command);
}