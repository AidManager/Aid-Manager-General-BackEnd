using System;
using System.Collections.Generic;
using System.Linq;
using AidManager.API.Authentication.Application.Internal.CommandServices;
using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Model.Commands;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.Shared.Domain.Repositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;

// using AidManager.API.Authentication.Application.Internal.CommandServices;
// using AidManager.API.Authentication.Domain.Model.Aggregates;
// using AidManager.API.Authentication.Domain.Repositories;
// using AidManager.API.Authentication.Domain.Services;
// using AidManager.API.Authentication.Domain.Model.Commands;
// using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.NUnitTests.Authentication
{
    [TestFixture]
    public class OrganizationCodeTests
    {
        [Test] // HU06/HU18: Código válido
        public void Test_Verified_Code_IsValid()
        {
            var repo = new Mock<ICompanyRepository>();
            var uow  = new Mock<IUnitOfWork>();
            var svc  = new CompanyCommandService(repo.Object, uow.Object);

            var cmd = new CreateCompanyCommand(
                CompanyName: "ONG X",
                Country:     "PE",
                Email:       "ong@example.org",
                UserId:      1
            );
            var company = new Company(cmd); 

            repo.Setup(r => r.FindCompanyByRegisterCode("ABC123"))
                .ReturnsAsync(company);

            var result = svc.Handle(new ValidateRegisterCode("ABC123"))
                .GetAwaiter().GetResult();

            result.Should().NotBeNull();
            result!.CompanyName.Should().Be("ONG X");
        }

        [Test] // HU06/HU18: Código inválido
        public void Test_Verified_Code_IsInvalid()
        {
            var repo = new Mock<ICompanyRepository>();
            var uow = new Mock<IUnitOfWork>();
            var svc = new CompanyCommandService(repo.Object, uow.Object);

            repo.Setup(r => r.FindCompanyByRegisterCode("BAD")).ReturnsAsync((Company?)null);

            Action act = () => svc.Handle(new ValidateRegisterCode("BAD")).GetAwaiter().GetResult();
            act.Should().Throw<Exception>().WithMessage("*Not Valid Register Code*");
        }
    }
}
