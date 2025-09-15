using AidManager.API.Authentication.Domain.Model.Entities;

namespace AidManager.API.ManageTasks.Interfaces.REST.Resources;

public record ProjectResource(int Id, DateOnly Audit ,string Name, string Description,DateOnly ProjectDate, TimeOnly ProjectTime, string ProjectLocation , int CompanyId, List<User> UserList , List<string> ImageUrl, double Rating);