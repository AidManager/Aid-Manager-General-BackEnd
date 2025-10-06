using System;
using AidManager.API.Authentication.Application.Internal.CommandServices;
using AidManager.API.Authentication.Application.Internal.QueryServices;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.Authentication.Domain.Services;
using AidManager.API.Authentication.Infrastructure.Persistence.EFC.Repositories;
using AidManager.API.Authentication.Interfaces.ACL;
using AidManager.API.Authentication.Interfaces.ACL.Services;
using AidManager.API.Authentication.Interfaces.REST.Resources;
using AidManager.API.Collaborate.Application.Internal.CommandServices;
using AidManager.API.Collaborate.Application.Internal.OutboundServices.ACL;
using AidManager.API.Collaborate.Application.Internal.QueryServices;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Collaborate.Infraestructure.Repositories;
using AidManager.API.IAM.Application.Internal.CommandServices;
using AidManager.API.IAM.Application.Internal.OutboundServices;
using AidManager.API.IAM.Application.Internal.QueryServices;
using AidManager.API.IAM.Domain.Repositories;
using AidManager.API.IAM.Domain.Services;
using AidManager.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using AidManager.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using AidManager.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using AidManager.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using AidManager.API.IAM.Infrastructure.Tokens.JWT.Services;
using AidManager.API.IAM.Interfaces.ACL;
using AidManager.API.IAM.Interfaces.ACL.Services;
using AidManager.API.ManageCosts.Application.Internal.CommandServices;
using AidManager.API.ManageCosts.Application.Internal.OutboundServices.ACL;
using AidManager.API.ManageCosts.Application.Internal.QueryServices;
using AidManager.API.ManageCosts.Domain.Repositories;
using AidManager.API.ManageCosts.Domain.Services;
using AidManager.API.ManageCosts.Infraestructure.Repositories;
using AidManager.API.ManageCosts.Interfaces.ACL;
using AidManager.API.ManageCosts.Interfaces.ACL.Services;
using AidManager.API.ManageTasks.Application.Internal.CommandServices;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices;
using AidManager.API.ManageTasks.Application.Internal.OutboundServices.ACL;
using AidManager.API.ManageTasks.Application.Internal.QueryServices;
using AidManager.API.ManageTasks.Domain.Repositories;
using AidManager.API.ManageTasks.Domain.Services;
using AidManager.API.ManageTasks.Infrastructure.Repositories;
using AidManager.API.ManageTasks.Interfaces.ACL;
using AidManager.API.ManageTasks.Interfaces.ACL.Services;
using AidManager.API.Payment.Application.Internal.CommandServices;
using AidManager.API.Payment.Application.Internal.QueryServices;
using AidManager.API.Payment.Domain.Repositories;
using AidManager.API.Payment.Domain.Services;
using AidManager.API.Payment.Infraestructure.Persistence.EFC.Repositories;
using AidManager.API.Shared.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Interfaces.ASP.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using AidManager.API.UserManagement.UserProfile.Application.Internal.OutboundServices.ACL;
using AidManager.API.UserProfile.Interfaces.ACL;
using AidManager.API.UserProfile.Interfaces.ACL.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AidManager.API.IAM.Application.Options;
using AidManager.API.IAM.Infrastructure.Security;
using AidManager.API.IAM.Interfaces.Security;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// configure kebab case route naming convention
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new KebabCaseRouteNamingConvention());
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
Console.WriteLine($"Starting server on port: {port}");
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "AidManager API Endpoints",
                Version = "v1",
                Description = "AidManager API Endpoints for managing tasks, costs, payments, and collaboration",
                TermsOfService = new Uri("https://aidmanager.com/tos"),
                Contact = new OpenApiContact
                {
                    Name = "Y.E.S.I Team",
                    Email = "contact@yesis.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });
        c.EnableAnnotations();
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer", Type = ReferenceType.SecurityScheme
                    } 
                }, 
                Array.Empty<string>()
            }
        });
    });

// adding database connection
Env.Load();

var connectionString = Environment.GetEnvironmentVariable("CONN_STRING");

// Configure Database Context and Logging Levels
builder.Services.AddDbContext<AppDBContext>(
    options =>
    {
        if (connectionString != null)
            if (builder.Environment.IsDevelopment())
                options.UseSqlServer(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            else if (builder.Environment.IsProduction())
                options.UseSqlServer(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Error)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();    
    });

// configure lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
      options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000", 
                "http://localhost:4200", 
                "https://aidmanagerv3.netlify.app",
                "https://aid-manager-general-backend-production.up.railway.app")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// shared bounded context injection configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// news bounded context injection configuration DEPENDENCY INJECTION
// post bounded context injection configuration
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostCommandService, PostCommandService>();
builder.Services.AddScoped<IPostQueryService, PostQueryService>();
builder.Services.AddScoped<ExternalUserAccountService>(); // Register ExternalUserAccountService

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentCommandService, CommentCommandService>();
builder.Services.AddScoped<ICommentQueryService, CommentQueryService>();


// event bounded context injection configuration
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventCommandService, EventCommandService>();
builder.Services.AddScoped<IEventQueryService, EventQueryService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddScoped<IAuthenticationFacade, AuthenticationFacade>();
builder.Services.AddScoped<ExternalUserAuthService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();

// analytics bounded context injection configuration
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
builder.Services.AddScoped<IAnalyticsCommandService, AnalyticsCommandService>();
builder.Services.AddScoped<IAnalyticsQueryService, AnalyticsQueryService>();
builder.Services.AddScoped<IManageCostsFacade, ManageCostsFacade>();
builder.Services.AddScoped<ExternalProjectsService>();


builder.Services.AddScoped<ITaskRepository, TaskItemsRepository>();
builder.Services.AddScoped<ITaskCommandService, TaskCommandService>();
builder.Services.AddScoped<ITaskQueryService, TaskQueryService>();
builder.Services.AddScoped<ExternalUserService>(); // Register ExternalUserService

//
builder.Services.AddScoped<IProjectCommandService, ProjectCommandService>();
builder.Services.AddScoped<IProjectQueryService, ProjectQueryService>();
builder.Services.AddScoped<IProjectRepository, ProjectsRepository>();
builder.Services.AddScoped<IManageTasksFacade, ManageTasksFacade>();

builder.Services.AddControllers();

// CAPTCHA 
builder.Services.Configure<ReCaptchaOptions>(
    builder.Configuration.GetSection("ReCaptcha"));
builder.Services.AddHttpClient<IReCaptchaValidator, ReCaptchaValidator>();


builder.Services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();
builder.Services.AddScoped<IPaymentDetailCommandService, PaymentDetailCommandService>();
builder.Services.AddScoped<IPaymentDetailQueryService, PaymentDetailQueryService>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyCommandService, CompanyCommandService>();
builder.Services.AddScoped<ICompanyQueryService, CompanyQueryService>();


builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserIAMRepository, UserIAMRepository>();
builder.Services.AddScoped<IUserIAMCommandService, UserIAMCommandService>();
builder.Services.AddScoped<IUserIAMQueryService, UserIAMQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IUserAccountFacade, UserAccountFacade>(); // Register IUserAccountFacade


builder.Services.AddScoped<IFavoritePostRepository, FavoritePostRepository>();
builder.Services.AddScoped<IFavoritePostCommandService, FavoritePostCommandService>();
builder.Services.AddScoped<IFavoritePostQueryService, FavoritePostQueryService>();

builder.Services.AddScoped<ILikedPostRepository, LikedPostRepository>();
builder.Services.AddScoped<IFavoriteProjects, FavoriteProjectsRepository>();

builder.Services.AddScoped<IDeletedUserRepository, DeletedUserRepository>();

builder.Services.AddScoped<ITaskEventHandlerService, TaskEventHandlerService>();

builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();



// Configure the HTTP request pipeline.
var app = builder.Build();

// verify database objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDBContext>();
    context.Database.EnsureCreated();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()|| app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure CORS - allow all origins temporarily for troubleshooting
Console.WriteLine("Configuring CORS to allow all origins");
app.UseCors(policy => 
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

// Add authorization middleware to pipeline
app.UseRequestAuthorization();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
