using System.Net.Mime;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.Queries;
using AidManager.API.ManageTasks.Domain.Services;
using AidManager.API.ManageTasks.Interfaces.REST.Resources;
using AidManager.API.ManageTasks.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AidManager.API.ManageTasks.Interfaces;

[ApiController]
[Route("api/v1/Projects/{projectId}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class TaskItemsController(ITaskCommandService taskCommandService, ITaskQueryService taskQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Task",
        Description = "Create a Project Task",
        OperationId = "CreateTaskItem"
    )]
    [SwaggerResponse(201, "Task Created", typeof(CreateTaskItemResource))]
    public async Task<ActionResult> CreateTaskItem(int projectId, [FromBody] CreateTaskItemResource resource)
    {
        try
        {
            var createTaskItemCommand = CreateTaskItemCommandFromResourceAssembler.ToCommandFromResource(resource, projectId);
            Console.WriteLine("The TaskItemController is called. and the Task Item is assembled." + createTaskItemCommand);
            var result = await taskCommandService.Handle(createTaskItemCommand);
            Console.WriteLine("The Create Item Command is handled in the taskCommandService.");
            var taskItemById = GetTaskItemById(result.Item1.Id);
            Console.WriteLine("Task by id called" + taskItemById.Result);
            var response = TaskItemResourceFromEntityAssembler.ToResourceFromEntity(result.Item1, result.Item2);
            return Ok(response);

        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }


    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a Task by Id",
        Description = "Get Task Item by using its ID",
        OperationId = "GetTask"
    )]
    public async Task<ActionResult<TaskItemResource>> GetTaskItemById(int id)
    {
        try
        {
            var taskItem = await taskQueryService.Handle(new GetTaskByIdQuery(id));
            var resource = TaskItemResourceFromEntityAssembler.ToResourceFromEntity(taskItem.Item1, taskItem.Item2);
            return Ok(resource);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }

    }


    [HttpPatch("{id}")]
    [SwaggerOperation(
        Summary = "Change Status",
        Description = "Update a Status Project Task",
        OperationId = "UpdateStatusTaskItem"
    )]
    [SwaggerResponse(201, "Task Updated", typeof(UpdateProjectStatusResource))]
    public async Task<ActionResult> ChangeStatusTaskItem(int id, int projectId, [FromBody] UpdateProjectStatusResource request)
    {
        try
        {
            var getTaskByIdQuery = new GetTaskByIdQuery(id);
            var taskItem = await taskQueryService.Handle(getTaskByIdQuery);
            taskItem.Item1.UpdateStatus(request.status);

            var updateTaskItemResource = new UpdateTaskItemResource(taskItem.Item1.Title, taskItem.Item1.Description, taskItem.Item1.DueDate, taskItem.Item1.State, taskItem.Item1.AssigneeId);

            var updateTaskCommand = UpdateTaskItemCommandFromResourceAssembler.ToCommandFromResource(updateTaskItemResource, id, projectId);

            var result = await taskCommandService.Handle(updateTaskCommand);

            return Ok(TaskItemResourceFromEntityAssembler.ToResourceFromEntity(result.Item1, result.Item2));
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
    }

    [HttpPut("edit/{id}")]
    [SwaggerOperation(
        Summary = "Update a Task",
        Description = "Update a Project Task",
        OperationId = "UpdateTaskItem"
    )]
    [SwaggerResponse(201, "Task Updated", typeof(UpdateTaskItemResource))]

    public async Task<ActionResult> UpdateTaskItem(int id, int projectId, [FromBody] UpdateTaskItemResource resource)
    {
        try
        {
            var updateTaskCommand = UpdateTaskItemCommandFromResourceAssembler.ToCommandFromResource(resource, id, projectId);
            var result = await taskCommandService.Handle(updateTaskCommand);
            return Ok(TaskItemResourceFromEntityAssembler.ToResourceFromEntity(result.Item1, result.Item2));
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);

        }

    }
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Task",
        Description = "Delete a ProjectTask",
        OperationId = "DeleteTaskItem"
    )]
    public async Task<ActionResult> DeleteTaskItem(int id, int projectId)
    {
        try
        {
            var deleteTaskItemCommand = new DeleteTaskCommand(id, projectId);
            var result = await taskCommandService.Handle(deleteTaskItemCommand);
            return Ok(TaskItemResourceFromEntityAssembler.ToResourceFromEntity(result.Item1, result.Item2));
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);

        }

    }

    [HttpGet("all")]
    [SwaggerOperation(
        Summary = "Get All Tasks from a project",
        Description = "Get all tasks from a project",
        OperationId = "GetAllTasks"
    )]
    public async Task<ActionResult<IEnumerable<TaskItemResource>>> GetAllTaskItems(int projectId)
    {
        try
        {
            var getAllTasksQueryByProjectId = new GetTasksByProjectIdQuery(projectId);
            var result = await taskQueryService.Handle(getAllTasksQueryByProjectId);
            var resources = result.Select(tuple => TaskItemResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2));
            return Ok(resources);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);

        }

    }

    [HttpGet("/api/v1/Company-Tasks/{companyId}")]
    [SwaggerOperation(
        Summary = "Get All Tasks from a Company",
        Description = "Get all tasks from a company",
        OperationId = "GetAllTasksByCompany"
    )]
    public async Task<ActionResult<IEnumerable<TaskItemResource>>> GetAllTaskItemsByCompany(int companyId)
    {
        try
        {
            var getAllTasksQueryByCompanyId = new GetTasksByCompanyId(companyId);
            var result = await taskQueryService.Handle(getAllTasksQueryByCompanyId);
            var resource = result.Select(tuple => TaskItemResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2));
            return Ok(resource);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);

        }

    }

    [HttpGet($"/api/v1/company-tasks/{{companyId}}/user/{{userId}}")]
    [SwaggerOperation(
        Summary = "Get All Tasks assigned User by Company",
        Description = "Get all tasks assigned to a user by company",
        OperationId = "GetAllTasksByUserByCompany"
    )]
    public async Task<ActionResult<IEnumerable<TaskItemResource>>> GetAllTaskItemsByUserByCompany(int companyId, int userId)
    {
        try
        {
            var getAllTasksQueryByUserIdByCompanyId = new GetAllTasksByUserIdByCompanyId(userId, companyId);
            var result = await taskQueryService.Handle(getAllTasksQueryByUserIdByCompanyId);
            var resources = result.Select(tuple => TaskItemResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2));
            return Ok(resources);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);

        }
    }

    [HttpGet($"/api/v1/user-tasks-project/{{projectId}}/user/{{userId}}")]
    [SwaggerOperation(
        Summary = "Get All Tasks by User and Project id",
        Description = "Get all tasks assigned to a user based on the project",
        OperationId = "GetAllTasksByUserAndProjectId"
    )]
    public async Task<ActionResult<IEnumerable<TaskItemResource>>> GetAllTaskItemsByUserAndProjectId(int userId, int projectId)
    {
        try
        {
            var getAllTasksQueryByUserIdAndProjectId = new GetTasksByUserIdAndProjectId(userId, projectId);
            var result = await taskQueryService.Handle(getAllTasksQueryByUserIdAndProjectId);
            var resources = result.Select(tuple => TaskItemResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2));
            return Ok(resources);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);

        }

    }

    [HttpGet("user/{userId}")]
    [SwaggerOperation(
        Summary = "Get All Tasks by User",
        Description = "Get all tasks assigned to a user",
        OperationId = "GetAllTasksByUser"
    )]
    public async Task<ActionResult<IEnumerable<TaskItemResource>>> GetAllTaskItemsByUser(int userId)
    {
        try
        {
            var getAllTasksQueryByUserId = new GetTasksByUserId(userId);
            var result = await taskQueryService.Handle(getAllTasksQueryByUserId);
            var resources = result.Select(tuple => TaskItemResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2));
            return Ok(resources);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);

        }
    }
    
}