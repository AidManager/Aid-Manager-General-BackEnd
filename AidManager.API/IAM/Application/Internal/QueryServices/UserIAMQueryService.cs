using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.IAM.Domain.Model.Aggregates;
using AidManager.API.IAM.Domain.Model.Queries;
using AidManager.API.IAM.Domain.Repositories;
using AidManager.API.IAM.Domain.Services;

namespace AidManager.API.IAM.Application.Internal.QueryServices;

public class UserIAMQueryService(IUserIAMRepository userRepository) : IUserIAMQueryService
{
    public async Task<UserAuth?> Handle(GetUserIAMByIdQuery query)
    {
        return await userRepository.FindByIdAsync(query.Id);
    }
    
    public async Task<UserAuth?> Handle(GetUserIAMByUsernameQuery query)
    {
        return await userRepository.FindByUsernameAsync(query.Username);
    }

    public async Task<IEnumerable<UserAuth>> Handle(GetAllUsersIAMQuery query)
    {
        return await userRepository.ListAsync();
    }
}