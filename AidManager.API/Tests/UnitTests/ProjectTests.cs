using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.ManageTasks.Domain.Model.Aggregates;
using AidManager.API.ManageTasks.Domain.Model.Commands;
using AidManager.API.Authentication.Domain.Model.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AidManager.Tests.CoreEntities
{
    [TestFixture]
    public class ProjectTests
    {
        [Test]
        public void UpdateRating_WithValidRating_ShouldUpdateRating()
        {
            // Arrange
            var project = new Project();

            // Act
            project.UpdateRating(4.5);

            // Assert
            Assert.That(project.Rating, Is.EqualTo(4.5));
        }

        [Test]
        public void CreateProject_WithValidCommand_ShouldSetAllProperties()
        {
            // Arrange
            var command = new CreateProjectCommand(
                "Test Project",
                "Project Description",
                new List<string> { "image1.jpg" },
                1,
                "2023-10-15",
                "14:30",
                "Test Location"
            );

            // Act
            var project = new Project(command);

            // Assert
            Assert.That(project.Name, Is.EqualTo("Test Project"));
            Assert.That(project.Description, Is.EqualTo("Project Description"));
            Assert.That(project.ImageUrl.Count, Is.EqualTo(1));
            Assert.That(project.ImageUrl[0].Url, Is.EqualTo("image1.jpg"));
            Assert.That(project.CompanyId, Is.EqualTo(1));
            Assert.That(project.ProjectDate, Is.EqualTo(DateOnly.Parse("2023-10-15")));
            Assert.That(project.ProjectTime, Is.EqualTo(TimeOnly.Parse("14:30")));
            Assert.That(project.ProjectLocation, Is.EqualTo("Test Location"));
            Assert.That(project.Rating, Is.EqualTo(0.00));
            Assert.That(project.AuditDate, Is.EqualTo(DateOnly.FromDateTime(DateTime.Now)));
        }

        [Test]
        public void AddImage_ShouldIncreaseImageCount()
        {
            // Arrange
            var project = new Project();
            var command = new AddProjectImageCommand(1, new List<string> { "new-image1.jpg", "new-image2.jpg" });

            // Act
            project.AddImage(command);

            // Assert
            Assert.That(project.ImageUrl.Count, Is.EqualTo(2));
            Assert.That(project.ImageUrl[0].Url, Is.EqualTo("new-image1.jpg"));
            Assert.That(project.ImageUrl[1].Url, Is.EqualTo("new-image2.jpg"));
        }
        
        [Test]
        public void UpdateRating_WithInvalidRating_ShouldThrowException()
        {
            // Arrange
            var project = new Project();

            // Act & Assert
            Assert.Throws<Exception>(() => project.UpdateRating(5.5));
            Assert.Throws<Exception>(() => project.UpdateRating(0.5));
        }
        
        [Test]
        public void UpdateProject_ShouldModifyAllProperties()
        {
            // Arrange
            var project = new Project();
            var command = new UpdateProjectCommand(
                1,
                "Updated Project",
                "Updated Description",
                new List<string> { "updated-image.jpg" },
                2,
                "2023-11-20",
                "16:45",
                "Updated Location"
            );

            // Act
            project.UpdateProject(command);

            // Assert
            Assert.That(project.Name, Is.EqualTo("Updated Project"));
            Assert.That(project.Description, Is.EqualTo("Updated Description"));
            Assert.That(project.ImageUrl.Count, Is.EqualTo(1));
            Assert.That(project.ImageUrl[0].Url, Is.EqualTo("updated-image.jpg"));
            Assert.That(project.CompanyId, Is.EqualTo(2));
            Assert.That(project.ProjectDate, Is.EqualTo(DateOnly.Parse("2023-11-20")));
            Assert.That(project.ProjectTime, Is.EqualTo(TimeOnly.Parse("16:45")));
            Assert.That(project.ProjectLocation, Is.EqualTo("Updated Location"));
        }
    }
}