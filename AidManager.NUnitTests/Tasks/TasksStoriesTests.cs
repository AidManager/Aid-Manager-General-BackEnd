using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Domain.Model.Queries;
using AidManager.API.ManageTasks.Application.Internal.QueryServices;
using AidManager.API.ManageTasks.Application.Internal.CommandServices;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.ManageTasks.Domain.Services;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices.ACL;

using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Authentication.Domain.Model.Commands;
using AidManager.API.ManageCosts.Interfaces.ACL;
using AidManager.API.Shared.Domain.Repositories;
using AidManager.API.UserProfile.Interfaces.ACL;

namespace AidManager.NUnitTests.Tasks
{
    [TestFixture]
    public class TasksStoriesTests
    {
        [Test] // HU12: Visualizar tareas (por proyecto)
        public async Task Test_Manager_View_Tasks()
        {
            var taskRepo     = new Mock<ITaskRepository>();
            var projectQuery = new Mock<IProjectQueryService>();

            // Facades para el ExternalUserService
            var accountFacade = new Mock<IUserAccountFacade>();
            var costsFacade   = new Mock<IManageCostsFacade>();

            accountFacade
                .Setup(x => x.GetUserById(It.IsAny<int>()))
                .ReturnsAsync(new User(new CreateUserCommand(
                    "U", "L", 20, "u@e", "+51", "x", "", 1, "", "", "", "C"
                )));

            costsFacade
                .Setup(x => x.CreateAnalytics(It.IsAny<int>()))
                .ReturnsAsync(new AidManager.API.ManageCosts.Domain.Model.Aggregates.Analytics());

            var external = new ExternalUserService(accountFacade.Object, costsFacade.Object);
            var querySvc = new TaskQueryService(taskRepo.Object, external, projectQuery.Object);

            var t1 = new TaskItem(new CreateTaskCommand(
                "A", "",
                DateOnly.FromDateTime(DateTime.Today),
                1, "ToDo", 7
            ));

            // genera la tarea de retorno
            taskRepo.Setup(r => r.GetTasksByProjectId(1))
                .ReturnsAsync([t1]);

            // lista la tarea
            List<(TaskItem Task, User User)> list =
                await querySvc.Handle(new GetTasksByProjectIdQuery(1));

            list.Should().HaveCount(1);

            var (task, user) = list.First();
            task.Title.Should().Be("A");
            task.State.Should().Be("ToDo");
        }

        [Test] // HU13: Crear tarea
        public void Test_Manager_Create_Tasks()
        {
            var cmd = new CreateTaskCommand(
                "Visita",
                "Recolectar datos",
                DateOnly.FromDateTime(DateTime.Today).AddDays(3),
                1,
                 "ToDo",
                 7
            );

            var t = new TaskItem(cmd);

            t.Title.Should().Be("Visita");
            t.State.Should().Be("ToDo");
            t.ProjectId.Should().Be(1);
        }

        [Test] // HU14: Cambio de estado de tarea
        public void Test_Collaborator_Change_Task_Status()
        {
            var t = new TaskItem(new CreateTaskCommand(
                "T", "D",
                DateOnly.FromDateTime(DateTime.Today),
                1, "ToDo", 7
            ));

            t.UpdateStatus("Done");
            t.State.Should().Be("Done");
        }

        [Test] // HU15: Actualizar/Eliminar tarea vía servicios de aplicación
        public void Test_Manager_Delete_Task()
        {
            var teamRepo     = new Mock<ITeamMemberRepository>();
            var taskRepo     = new Mock<ITaskRepository>();
            var projRepo     = new Mock<IProjectRepository>();
            var uow          = new Mock<IUnitOfWork>();
            var eventHandler = new Mock<ITaskEventHandlerService>();
            
            // Facades para el ExternalUserService
            var accountFacade = new Mock<IUserAccountFacade>();
            var costsFacade   = new Mock<IManageCostsFacade>();

            accountFacade
                .Setup(x => x.GetUserById(It.IsAny<int>()))
                .ReturnsAsync(new User(new CreateUserCommand(
                    "U", "L", 20, "u@e", "+51", "x", "", 1, "", "", "", "C"
                )));

            costsFacade
                .Setup(x => x.CreateAnalytics(It.IsAny<int>()))
                .ReturnsAsync(new AidManager.API.ManageCosts.Domain.Model.Aggregates.Analytics());

            var external = new ExternalUserService(accountFacade.Object, costsFacade.Object);

            var cmdSvc = new TaskCommandService(
                teamRepo.Object,
                eventHandler.Object,
                taskRepo.Object,
                uow.Object,
                projRepo.Object,
                external
            );

            projRepo.Setup(r => r.ExistsProject(1)).ReturnsAsync(true);

            taskRepo.Setup(r => r.GetTaskById(5))
                    .ReturnsAsync(new TaskItem(new CreateTaskCommand(
                        "T", "D", DateOnly.FromDateTime(DateTime.Today),
                        1, "ToDo", 2
                    )));


            var res = cmdSvc.Handle(new DeleteTaskCommand( 5,  1))
                            .GetAwaiter().GetResult();

            taskRepo.Verify(r => r.Remove(It.IsAny<TaskItem>()), Times.Once);
            uow.Verify(x => x.CompleteAsync(), Times.Once);
        }


    }
}