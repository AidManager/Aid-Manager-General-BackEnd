using System.Diagnostics.Tracing;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AidManager.API.Collaborate.Application.Internal.CommandServices;

public class EventCommandService(IEventRepository eventRepository) : IEventCommandService
{
    
    
    public async Task<Boolean> handle(CreateEventCommand command)
    {
        try
        {
            var eventEntity = new Event(command);
            var result = await eventRepository.AddAsync(eventEntity);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            return false;
        }
    }

    public async Task<Boolean> handle(EditEventCommand command)
    {
        try
        {
            var eventEntity = await eventRepository.GetEventById(command.Id);
            if (eventEntity == null)
            {
                return false;
            }
            
            eventEntity.Name = command.Name;
            eventEntity.Description = command.Description;
            eventEntity.Location = command.Location;
            var result = await eventRepository.Update(eventEntity);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error to obtain event by id" + e.Message);
            return false;
        }
    }

    public async Task<Boolean> handle(DeleteEventCommand command)
    {
        try
        {
            var eventEntity = await eventRepository.GetEventById(command.eventId);
            if (eventEntity == null)
            {
                return false;
            }

            var result = await eventRepository.Remove(eventEntity);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error removing event" + e.Message);
            return false;
        }
    } 
}