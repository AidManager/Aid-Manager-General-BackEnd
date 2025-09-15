using AidManager.API.IAM.Domain.Model.Aggregates;
using AidManager.API.IAM.Domain.Model.Commands;

namespace AidManager.API.IAM.Domain.Services;

public interface IUserIAMCommandService
{
    Task Handle(DeleteUserCommand command);
    Task Handle(UpdateUserCommand command);
    Task Handle(SignUpCommand command);
    Task<(UserAuth user, string token)> Handle(SignInCommand command);
}