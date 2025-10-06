using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Authentication.Domain.Model.Queries;

namespace AidManager.API.Authentication.Domain.Services;

public interface IUserQueryService
{
    Task<IEnumerable<User>?> Handle(GetAllUsersByCompanyIdQuery byCompanyIdQuery);
    
    Task<User?> FindUserById(GetUserByIdQuery id);
    Task<User?> FindUserByEmail(GetUserByEmailQuery email);

    Task<IEnumerable<DeletedUser?>> HandleDel(GetAllUsersByCompanyIdQuery query);

}