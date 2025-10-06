using AidManager.API.IAM.Domain.Model.Commands;
using AidManager.API.IAM.Domain.Model.Queries;
using AidManager.API.IAM.Domain.Services;
using AidManager.API.UserProfile.Domain.Model.Commands;
using UpdateUserCommand = AidManager.API.IAM.Domain.Model.Commands.UpdateUserCommand;

namespace AidManager.API.IAM.Interfaces.ACL.Services;

public class IamContextFacade(IUserIAMCommandService userCommandService, IUserIAMQueryService userQueryService): IIamContextFacade
{
    public async Task DeleteUser(string username)
    {
        var deleteUserCommand = new DeleteUserCommand(username);
        await userCommandService.Handle(deleteUserCommand);
    }

    public async Task<int> CreateUser(string username, string password, int role)
    {
        var signUpCommand = new SignUpCommand(username, password,role );
        await userCommandService.Handle(signUpCommand);
        var getUserByUsernameQuery = new GetUserIAMByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }
    
    public async Task<int> UpdateUserData(string oldUsername,string username, string password, int role)
    {
        var updateUserCommand = new UpdateUserCommand(oldUsername,username, password, role);
        await userCommandService.Handle(updateUserCommand);
        var getUserByUsernameQuery = new GetUserIAMByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<int> FetchUserIdByUsername(string username)
    {
        var getUserByUsernameQuery = new GetUserIAMByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<string> FetchUsernameByUserId(int userId)
    {
        var getUserByIdQuery = new GetUserIAMByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery);
        return result?.Username ?? string.Empty;
    }
}