using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;

namespace AidManager.API.Collaborate.Domain.Services;

public interface ICommentCommandService
{
    Task<(Comments?,User)> Handle(DeleteCommentCommand command);
    Task<(Comments?,User)> Handle(AddCommentCommand command);
}