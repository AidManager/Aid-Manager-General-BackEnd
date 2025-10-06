using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.UserProfile.Domain.Model.Commands;
using AidManager.API.Authentication.Application.Internal.QueryServices;
using AidManager.API.Authentication.Domain.Model.Commands;
using AidManager.API.Authentication.Domain.Services;
using AidManager.API.Authentication.Domain.Model.Queries;
using AidManager.API.Authentication.Domain.Repositories;

namespace AidManager.NUnitTests.Users
{
    [TestFixture]
    public class UsersStoriesTests
    {
        [Test] // HU04: Mostrar información de perfiles de gestores
        public void Test_View_Manager_Information()
        {
            var create = new CreateUserCommand(
                "Ana","Pérez",28,"ana@example.org","+51 1","Sec!","img.png",
                0,"ONG X","ong@example.org","PE","CODE123");
            var user = new User(create);

            user.FirstName.Should().Be("Ana");
            user.Email.Should().Be("ana@example.org");
            user.Role.ToString().Should().Be("0");
        }

        [Test] // HU05: Editar información de perfil
        public void Test_Edit_Profile()
        {
            var create = new CreateUserCommand(
                "Ana","Pérez",28,"ana@example.org","+51 1","Sec!","img.png",
                0,"ONG X","ong@example.org","PE","CODE123");
            var user = new User(create);
            var update = new UpdateUserCommand("Ana María","Pérez",29,"+51 2","new.png","ana@example.org","Sec!");

            user.updateProfile(update);

            user.FirstName.Should().Be("Ana María");
            user.Age.Should().Be(29);
            user.Phone.Should().Be("+51 2");
            user.ProfileImg.Should().Be("new.png");
        }
        
        [Test] // HU07: Mostrar lista de perfiles de miembros de equipo (QueryService mockeado)
        public void Test_Manager_List_Profiles()
        {
            var repo = new Mock<IUserRepository>();
            var delRepo = new Mock<IDeletedUserRepository>();
            var q = new UserQueryService(repo.Object, delRepo.Object);

            var u1 = new User(new CreateUserCommand(
                "Ana","Pérez",28,"ana@example.org","+51 1","x","img",
                1,"","","","C1")) { CompanyId = 10 };
            var u2 = new User(new CreateUserCommand(
                "Luis","Rojas",31,"luis@example.org","+51 2","x","img",
                1,"","","","C1")) { CompanyId = 10 };

            repo.Setup(r => r.FindUsersByCompanyId(10)).ReturnsAsync([u1, u2]);

            var result = q.Handle(new GetAllUsersByCompanyIdQuery(10)).GetAwaiter().GetResult();
            if (result == null) return;
            var iEnumerable = result as User[] ?? result.ToArray();
            iEnumerable.Should().NotBeNull();
            iEnumerable!.Length.Should().Be(2);
        }

        [Test] // HU08: Eliminar perfil de miembro de equipo (borrado lógico a DeletedUser)
        public void Test_Manager_Remove_Profile()
        {
            var create = new CreateUserCommand(
                "Luis","Rojas",31,"luis@example.org","+51 3","x","pic.png",
                1,"","","","CODE123");
            var user = new User(create);

            user.DeleteUser();

            user.FirstName.Should().Be("Deleted");
            // user.LastName.Should().Be("Rojas");
            // user.Age.Should().Be(31);
        }
    }
}
