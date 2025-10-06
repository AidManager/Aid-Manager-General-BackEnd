using System;
using System.Text.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AidManager.BDD.Steps
{
    [Binding]
    public class ProjectsTasksSteps
    {
        private readonly Support.World _world;
        public ProjectsTasksSteps(Support.World world) => _world = world;

        [Given(@"registré un TeamMember con ese TeamRegisterCode")]
        public async Task GivenRegistroTeamMember()
        {
            var payload = new {
                FirstName = "Jose",
                LastName = "Chavez",
                Age = 26,
                Email = "jose@example.org",
                Phone = "+51 7",
                Password = "p",
                ProfileImg = "img",
                Role = 1,
                CompanyName = "",
                CompanyEmail = "",
                CompanyCountry = "",
                TeamRegisterCode = _world.TeamRegisterCode
            };
            var resp = await _world.Client.PostAsJsonAsync("/api/v1/Users/sign-up", payload);
            var json = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            _world.AssigneeId = json.RootElement.GetProperty("data").GetProperty("Id").GetInt32();
        }

        [When(@"creo un proyecto válido")]
        public async Task WhenCreoProyectoValido()
        {
            var payload = new {
                Name = "Campaña Salud",
                Description = "Jornadas médicas",
                ImageUrl = new [] { "a.png" },
                CompanyId = _world.CompanyId,
                ProjectDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
                ProjectTime = TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm"),
                ProjectLocation = "Lima"
            };
            _world.LastResponse = await _world.Client.PostAsJsonAsync("/api/v1/Projects", payload);
            _world.LastJson = JsonDocument.Parse(await _world.LastResponse.Content.ReadAsStringAsync());
        }

        [Then(@"guardo el ProjectId")]
        public void ThenGuardoProjectId()
        {
            _world.ProjectId = _world.LastJson!.RootElement.GetProperty("Id").GetInt32();
            _world.ProjectId.Should().BeGreaterThan(0);
        }

        [When(@"creo una tarea ToDo para ese proyecto asignada al TeamMember")]
        public async Task WhenCreoTarea()
        {
            var payload = new {
                Title = "Visitar comunidad",
                Description = "Recolectar datos",
                DueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(3),
                State = "ToDo",
                AssigneeId = _world.AssigneeId
            };
            _world.LastResponse = await _world.Client.PostAsJsonAsync($"/api/v1/Projects/{_world.ProjectId}/TaskItems", payload);
            _world.LastJson = JsonDocument.Parse(await _world.LastResponse.Content.ReadAsStringAsync());
        }

        [Then(@"guardo el TaskId con estado ToDo")]
        public void ThenGuardoTaskId()
        {
            _world.TaskId = _world.LastJson!.RootElement.GetProperty("Id").GetInt32();
            _world.LastJson!.RootElement.GetProperty("State").GetString().Should().Be("ToDo");
        }

        [When(@"consulto las tareas del proyecto")]
        public async Task WhenConsultoTareas()
        {
            _world.LastResponse = await _world.Client.GetAsync($"/api/v1/Projects/{_world.ProjectId}/TaskItems/all");
        }

        [When(@"actualizo la tarea a estado Done")]
        public async Task WhenActualizoTareaDone()
        {
            var payload = new {
                Title = "Visitar comunidad",
                Description = "Recolectar datos",
                DueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(4),
                State = "Done",
                AssigneeId = _world.AssigneeId
            };
            _world.LastResponse = await _world.Client.PutAsJsonAsync($"/api/v1/Projects/{_world.ProjectId}/TaskItems/edit/{_world.TaskId}", payload);
        }

        [When(@"elimino la tarea")]
        public async Task WhenEliminoTarea()
        {
            _world.LastResponse = await _world.Client.DeleteAsync($"/api/v1/Projects/{_world.ProjectId}/TaskItems/{_world.TaskId}");
        }
    }
}
