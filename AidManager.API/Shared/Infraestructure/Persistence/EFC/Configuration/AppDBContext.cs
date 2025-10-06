using AidManager.API.Authentication.Domain.Model.Aggregates;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Aggregates;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.IAM.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Domain.Model.Entities;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.ValueObjects;
using AidManager.API.Payment.Domain.Model.Aggregates;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions options) : base(options){}
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // this method is to configure the database schema and the tables
        base.OnModelCreating(builder);
        
        // here we can configure the tables for post
        builder.Entity<Post>().ToTable("Posts");
        builder.Entity<Post>().HasKey(p => p.Id);
        builder.Entity<Post>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Post>().Property(p => p.CreatedAt).IsRequired();
        builder.Entity<Post>().Property(p => p.UserId).HasColumnName("user_id"); 
        builder.Entity<Post>().Property(p => p.CompanyId).HasColumnName("company_id");
        builder.Entity<Post>().Property(p => p.Title).IsRequired();
        builder.Entity<Post>().HasMany(p => p.ImageUrl).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Post>().HasMany(p => p.Comments).WithOne().OnDelete(DeleteBehavior.Cascade);
        
        // table for favorite posts
        
        builder.Entity<FavoritePosts>().ToTable("FavoritePosts");
        builder.Entity<FavoritePosts>().HasKey(fp => new { fp.UserId, fp.PostId });
        
        // table for liked posts
        builder.Entity<LikedPosts>().ToTable("LikedPosts");
        builder.Entity<LikedPosts>().HasKey(lp => new { lp.UserId, lp.PostId });
        
        // table for favorite projects
        
        builder.Entity<FavoriteProjects>().ToTable("FavoriteProjects");
        builder.Entity<FavoriteProjects>().HasKey(fp => new { fp.UserId, fp.ProjectId });
        
        // table for events
        builder.Entity<Event>().ToTable("Events");
        builder.Entity<Event>().HasKey(e => e.Id);
        builder.Entity<Event>().Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.Entity<User>().ToTable("Accounts");
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        
        // table for analytics
        builder.Entity<Analytics>().ToTable("Analytics");
        builder.Entity<Analytics>().HasKey(a => a.Id);
        builder.Entity<Analytics>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Analytics>().OwnsMany(a => a.BarData, b =>
        {
            b.WithOwner().HasForeignKey("AnalyticsId");
            b.HasKey("AnalyticsId");
        });
        builder.Entity<Analytics>().OwnsMany(a => a.LinesChartBarData, b =>
        {
            b.WithOwner().HasForeignKey("AnalyticsId");
            b.HasKey("AnalyticsId");
        });


        
        builder.Entity<TaskItem>().ToTable("Tasks");
        builder.Entity<TaskItem>().HasKey(t => t.Id);
        builder.Entity<TaskItem>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<TaskItem>().Property(p => p.DueDate).IsRequired().HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v)
        ); // Ensure correct mapping
        builder.Entity<TaskItem>().Property(p => p.CreatedAt).IsRequired().HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v)
            );
        
        
        builder.Entity<Project>().ToTable("Projects");
        builder.Entity<Project>().HasKey(p => p.Id);
        builder.Entity<Project>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Project>().HasMany(p => p.ImageUrl).WithOne().OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProjectTeamMembers>().ToTable("ProjectUser");
        builder.Entity<ProjectTeamMembers>().HasKey(ptm => new { ptm.ProjectId, ptm.UserId });
        
        
        builder.Entity<Project>().Property(p => p.ProjectDate).IsRequired().HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v)
        );
        
        builder.Entity<Project>().Property(p => p.AuditDate).IsRequired().HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v)
        );
        
        builder.Entity<Project>().Property(p => p.ProjectTime).IsRequired().HasConversion(
            v => v.ToTimeSpan(),
            v => TimeOnly.FromTimeSpan(v)
        ); // Ensure correct mapping        
        builder.Entity<ProjectImage>().ToTable("ProjectImages");
        builder.Entity<ProjectImage>().HasKey(pi => pi.Id);
        builder.Entity<ProjectImage>().Property(pi => pi.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ProjectImage>().Property(pi => pi.Url).IsRequired();
        
        builder.Entity<PostImage>().ToTable("PostsImages");
        builder.Entity<PostImage>().HasKey(pi => pi.Id);
        builder.Entity<PostImage>().Property(pi => pi.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<PostImage>().Property(pi => pi.PostImageUrl).IsRequired();
        
        builder.Entity<Comments>().ToTable("Comments");
        builder.Entity<Comments>().HasKey(c => c.Id);
        builder.Entity<Comments>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Comments>().Property(c => c.Comment).IsRequired();
        builder.Entity<Comments>().Property(c => c.UserId).IsRequired();
        builder.Entity<Comments>().Property(c => c.PostId).IsRequired();

        //Favorite Projects
        builder.Entity<FavoriteProjects>().ToTable("FavoriteProjects");
        builder.Entity<FavoriteProjects>().HasKey(fp => new { fp.UserId, fp.ProjectId });
        
        
        builder.Entity<PaymentDetail>().ToTable("PaymentDetails");
        builder.Entity<PaymentDetail>().HasKey(p => p.Id);
        builder.Entity<PaymentDetail>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.Entity<Company>().ToTable("Companies");
        builder.Entity<Company>().HasKey(c => c.Id);
        builder.Entity<Company>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        
        
        // iam context
        builder.Entity<UserAuth>().ToTable("Users");
        builder.Entity<UserAuth>().HasKey(u => u.Id);
        builder.Entity<UserAuth>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<UserAuth>().Property(u => u.Username).IsRequired();
        builder.Entity<UserAuth>().Property(u => u.PasswordHash).IsRequired();
        
        //Deleted Users
        
        builder.Entity<DeletedUser>().ToTable("DeletedAccounts");
        builder.Entity<DeletedUser>().HasKey(u => u.Id);
        
        builder.UseSnakeCaseNamingConvention();
    }
}