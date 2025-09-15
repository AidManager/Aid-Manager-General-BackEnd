using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Authentication.Domain.Repositories;

public interface IDeletedUserRepository : IBaseRepository<DeletedUser>
{
        Task<List<DeletedUser>> FindUsersByCompanyId(int companyId);
        
        Task<DeletedUser?> FindUserByEmail(string email);
        
        Task<DeletedUser?> FindUserById(int id);
}