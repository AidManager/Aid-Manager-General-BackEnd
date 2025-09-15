using AidManager.API.Collaborate.Domain.Model.Commands;

namespace AidManager.API.Collaborate.Domain.Services;

public interface IEventCommandService
{
    Task<Boolean> handle(CreateEventCommand command);
    Task<Boolean> handle(EditEventCommand command);
    Task<Boolean> handle(DeleteEventCommand command);
}