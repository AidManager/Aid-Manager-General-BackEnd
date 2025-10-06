using System;
using System.Text.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AidManager.BDD.Steps
{
    [Binding]
    public class CalendarAnalyticsSteps
    {
        private readonly Support.World _world;
        public CalendarAnalyticsSteps(Support.World world) => _world = world;

        [Given(@"creo un proyecto válido y guardo su ProjectId")]
        public async Task GivenCreoProyecto()
        {
            var payload = new {
                Name = "Proyecto Z",
                Description = "Desc",
                ImageUrl = Array.Empty<string>(),
                CompanyId = _world.CompanyId,
                ProjectDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
                ProjectTime = TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm"),
                ProjectLocation = "Cusco"
            };
            var resp = await _world.Client.PostAsJsonAsync("/api/v1/Projects", payload);
            var json = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            _world.ProjectId = json.RootElement.GetProperty("Id").GetInt32();
        }

        [When(@"creo un evento para el proyecto")]
        public async Task WhenCreoEvento()
        {
            var payload = new {
                Name = "Reunión",
                Date = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
                Location = "Lima",
                Description = "Detalle",
                Color = "rojo",
                ProjectId = _world.ProjectId
            };
            _world.LastResponse = await _world.Client.PostAsJsonAsync("/api/v1/Events", payload);
        }

        [When(@"creo analytics para el proyecto")]
        public async Task WhenCreoAnalytics()
        {
            var lines = new [] {
                new { Name = "Ene", Value = 10.0 },
                new { Name = "Feb", Value = 12.0 }
            };
            var bars = new [] {
                new { Name = "A", Value = 3.0 },
                new { Name = "B", Value = 4.0 }
            };
            var payload = new {
                ProjectId = _world.ProjectId,
                LinesChartBarData = lines,
                BarData = bars,
                Progressbar = new [] { 0.2, 0.4, 0.6 },
                Status = new [] { 1.0, 0.0, 1.0 },
                Tasks = new [] { 5.0, 7.0, 8.0 }
            };
            _world.LastResponse = await _world.Client.PostAsJsonAsync("/api/v1/Analytics", payload);
        }

        [Then(@"la respuesta es 200 OK")]
        public void ThenOk()
        {
            _world.LastResponse!.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
