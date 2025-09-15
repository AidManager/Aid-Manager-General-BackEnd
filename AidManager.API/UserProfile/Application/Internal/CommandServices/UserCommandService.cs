using AidManager.API.Authentication.Domain.Model.Commands;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.Authentication.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;
using AidManager.API.UserManagement.UserProfile.Application.Internal.OutboundServices.ACL;
using AidManager.API.UserProfile.Domain.Model.Commands;
using Microsoft.EntityFrameworkCore.Storage;

namespace AidManager.API.Authentication.Application.Internal.CommandServices;

public class UserCommandService(IDeletedUserRepository deletedUserRepository,IUserRepository userRepository, IUnitOfWork unitOfWork, ExternalUserAuthService externalUserAuthService) : IUserCommandService
{
    public async Task<User?> Handle(CreateUserCommand command)
    {
        string? externalUserEmail = null;
        try
        {
            var validate = await userRepository.FindUserByEmail(command.Email);
            var companyByEmail = await externalUserAuthService.GetCompanyByEmail(command.CompanyEmail);
            if (companyByEmail != null && command.Role == 0)
            {
                Console.WriteLine("Company EMAIL ALREADY USED"); 
                throw new Exception("Error: Company EMAIL already exist");
            } 
            if (validate != null) 
            {
                Console.WriteLine("EMAIL ALREADY USED"); 
                throw new Exception("Error: User EMAIL already exists");
            }

            var user = new User(command);

            // Solo después de validaciones locales, crear usuario externo
            switch (command.Role)
            {
                //Manager
                case 0:
                    await externalUserAuthService.CreateUsername(user.Email, user.Password, user.Role);
                    externalUserEmail = user.Email;
                    var userid = await externalUserAuthService.FetchUserIdByUsername(user.Email);
                    var company = await externalUserAuthService.CreateCompany(command.CompanyName, command.CompanyCountry, command.CompanyEmail, userid);
                    user.CompanyId = company.Id;
                    break;
                //TeamMember
                case 1:
                    var companyData = await externalUserAuthService.AuthenticateCode(command.TeamRegisterCode); 
                    await externalUserAuthService.CreateUsername(user.Email, user.Password, user.Role);
                    externalUserEmail = user.Email;
                    user.CompanyId = companyData.Id;
                    break;
            }

            Console.WriteLine("USER: " + user);

            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in creation: " + e.Message);
            // Si ya se creó el usuario externo, intentar eliminarlo
            if (externalUserEmail != null)
            {
                try
                {
                    await externalUserAuthService.DeleteUser(externalUserEmail);
                }
                catch (Exception cleanupEx)
                {
                    Console.WriteLine($"Error cleaning up external user: {cleanupEx.Message}");
                }
            }
            throw;
        }
        
    }
    
    public async Task<User> Handle(UpdateUserCommand command, int id)
    {
        var user = await userRepository.FindUserById(id);
        
        if (user != null)
        {
            var oldUsername = user.Email;
            user.updateProfile(command);
            await externalUserAuthService.UpdateUser(oldUsername,user.Email, user.Password, user.Role);
                    await userRepository.Update(user);
                    await unitOfWork.CompleteAsync();
                    return user;
        }
        throw new Exception("Could not update: User not found");
    }
    
    
        
    public async Task<bool> Handle(KickUserByCompanyIdCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user != null)
        {
            await externalUserAuthService.DeleteUser(user.Email);

            var deletedUser = new DeletedUser(user);
            await deletedUserRepository.AddAsync(deletedUser);
            user.DeleteUser();
            await userRepository.Update(user);
            await unitOfWork.CompleteAsync();
            return true;
        }

        return false;
    }

    public async Task<User?> Handle(PatchImageCommand command, int userId)
    {
         
            var user = await userRepository.FindUserById(userId);
            if (user != null)
            {
                user.updateImage(command);
                await userRepository.Update(user);
                await unitOfWork.CompleteAsync();
                return user;
            }
            throw new Exception("Could not update Image: User not found");
    }
    
    
}