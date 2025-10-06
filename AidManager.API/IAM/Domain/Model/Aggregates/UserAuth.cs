using System.Text.Json.Serialization;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.IAM.Domain.Model.ValueObjects;

namespace AidManager.API.IAM.Domain.Model.Aggregates;

public class UserAuth(string username, string passwordHash, int userRole)
{
    public UserAuth() : this(string.Empty, string.Empty, 0) { }
    
    public int Id { get;  }

    public string Username { get; private set; } = username;
    
    public UserRole Role { get; private set; } = (UserRole)userRole;
    
    [JsonIgnore] public string PasswordHash { get; private set; } = passwordHash;
    
    public UserAuth UpdateUsername(string username)
    {
        Username = username;
        return this;
    }
    
    public UserAuth UpdateRole(int role)
    {
        Role = (UserRole)role;
        return this;
    }
    public UserAuth UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }
    
}