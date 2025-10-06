using System;
using System.Linq;
using AidManager.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;

namespace AidManager.BDD
{
    public class AidManagerFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDBContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<AppDBContext>(opt => opt.UseInMemoryDatabase($"aidmanager-bdd-{Guid.NewGuid()}"));
            });
        }
    }
}
