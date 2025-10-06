using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;

namespace AidManager.API.Collaborate.Domain.Services;

public interface ICommentQueryService
{
    Task<List<(Comments?, User)>> Handle(GetCommentsByPostIdQuery query);
    Task<List<(Comments?, User)>> Handle(GetCommentsByUserIdQuery query);
    Task<(Comments?,User)> Handle(GetCommentsByIdQuery query);

}