namespace AidManager.API.IAM.Interfaces.ACL;

public interface IIamContextFacade
{
    
    Task DeleteUser(string username);
    Task<int> CreateUser(string username, string password, int role);
    Task<int> FetchUserIdByUsername(string username);
    
    Task<int> UpdateUserData(string oldUsername,string username, string password, int role);
    
    Task<string> FetchUsernameByUserId(int userId);
}