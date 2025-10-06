using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.IAM.Application.Internal.OutboundServices;
using AidManager.API.IAM.Domain.Model.Aggregates;
using AidManager.API.IAM.Domain.Model.Commands;
using AidManager.API.IAM.Domain.Repositories;
using AidManager.API.IAM.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.IAM.Application.Internal.CommandServices;

public class UserIAMCommandService(
    IUserIAMRepository userRepository,
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IHashingService hashingService
    ) : IUserIAMCommandService
{
    public async Task Handle(SignUpCommand command)
    {
        if (userRepository.ExistsByUsername(command.Username))
            throw new Exception($"Username {command.Username} is already taken");
        
        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new UserAuth(command.Username, hashedPassword, command.UserRole);
        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating the user: {e.Message}");
        }
    }

    public async Task Handle(DeleteUserCommand command)
    {
        try
        {
            var user = await userRepository.FindByUsernameAsync(command.Username);
            await userRepository.Remove(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Handle(UpdateUserCommand command)
    {
        var user = await userRepository.FindByUsernameAsync(command.OldUsername);
        
        if (user is null)
            throw new Exception($"User with username {command.Username} does not exist");
        
        var hashedPassword = hashingService.HashPassword(command.Password);
        user.UpdatePasswordHash(hashedPassword);
        user.UpdateUsername(command.Username);
        user.UpdateRole(command.UserRole);
        
        
        try
        {
            await userRepository.Update(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while updating the user: {e.Message}");
        }
    }
    
    public async Task<(UserAuth user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username);
        
        if (user is null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        var token = tokenService.GenerateToken(user);

        return (user, token);
    }
}