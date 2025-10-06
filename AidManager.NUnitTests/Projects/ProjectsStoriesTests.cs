using System;
using System.Collections.Generic;
using System.Linq;
using AidManager.API.Authentication.Domain.Model.Commands;
using FluentAssertions;
using Moq;
using NUnit.Framework;

using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.ManageTasks.Application.Internal.QueryServices;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.ManageTasks.Domain.Services;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Shared.Domain.Repositories;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices.ACL;
using AidManager.API.ManageTasks.Domain.Model.Queries;
using AidManager.API.UserProfile.Interfaces.ACL;
using AidManager.API.ManageCosts.Interfaces.ACL;

namespace AidManager.NUnitTests.Projects
{
    [TestFixture]
    public class ProjectsStoriesTests
    {
        [Test] // HU09: Visualizar listado de proyectos
        public async Task Test_View_Projects()
        {
            var unit     = new Mock<IUnitOfWork>();
            var teamRepo = new Mock<ITeamMemberRepository>();
            var favRepo  = new Mock<IFavoriteProjects>();
            var projRepo = new Mock<IProjectRepository>();

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

            var querySvc = new ProjectQueryService(
                unit.Object,
                teamRepo.Object,
                favRepo.Object,
                projRepo.Object,
                external 
            );

            var create = new CreateProjectCommand(
                "P1", "D", new List<string>(), // si usas C# 12 puedes poner [] en lugar de new List<string>()
                10,
                DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
                "10:00",
                "Lima"
            );
            var project = new Project(create) { Id = 10 };

            projRepo.Setup(r => r.GetProjectsByCompanyId(10))!
                .ReturnsAsync([project]);

            teamRepo.Setup(r => r.GetTeamMembers(10))
                .ReturnsAsync([]);

            var list = await querySvc.Handle(new GetAllProjectsQuery(10));

            var valueTuples = list as (Project, List<User>)[] ?? list.ToArray();
            valueTuples.Should().HaveCount(1);
            valueTuples.First().Item1.Name.Should().Be("P1");
        }

        [Test] // HU10: Ingresar nuevo proyecto
        public void Test_Manager_Create_Project()
        {
            var cmd = new CreateProjectCommand(
                "Campaña Salud",
                "Atención primaria",
                new List<string> { "img1.png" }, // usa [] si tu proyecto tiene C# 12 habilitado
                10,
                DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
                TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm"),
                "Lima"
            );
            var p = new Project(cmd);

            p.Name.Should().Be("Campaña Salud");
            p.CompanyId.Should().Be(10);
            p.ImageUrl.Should().HaveCount(1);
            p.Rating.Should().Be(0.0);
            p.AuditDate.Should().BeOnOrAfter(DateOnly.FromDateTime(DateTime.Today.AddDays(-1)));
        }

        [Test] // HU11: Editar información de proyecto
        public void Test_Manager_Update_Project()
        {
            var cmd = new CreateProjectCommand(
                "Campaña Salud",
                "Atención primaria",
                new List<string> { "img1.png" },
                10,
                DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
                TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm"),
                "Lima"
            );
            var p = new Project(cmd);

            var u = new UpdateProjectCommand(
                ProjectId: p.Id,
                Name: "Nuevo",
                Description: "Actualizado",
                ImageUrl: new List<string> { "nueva.png" },
                CompanyId: 10,
                ProjectDate: DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
                ProjectTime: "10:00",
                ProjectLocation: "Cusco"
            );

            p.UpdateProject(u);

            p.Name.Should().Be("Nuevo");
            p.Description.Should().Be("Actualizado");
            p.ProjectLocation.Should().Be("Cusco");
        }
    }
}
