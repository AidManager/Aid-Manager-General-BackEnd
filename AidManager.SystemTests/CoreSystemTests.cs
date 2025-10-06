using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace AidManager.SystemTests 
{
    [TestFixture]
    [Category("System")]
    public class CoreSystemTests
    {
        private AidManagerFactory _factory = null!;
        private HttpClient _client = null!;

        [SetUp]
        public void Setup()
        {
            _factory = new AidManagerFactory();
            _client  = _factory.CreateClient();
        }

        [TearDown]
        public void Teardown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [Test]
        public async Task EndToEnd_Manager_Project_Task_Event_Analytics()
        {
            // 1) Crear un manager para tener companyId y teamCode
            var mgrPayload = new
            {
                FirstName = "Marta",
                LastName = "Suárez",
                Age = 30,
                Email = "marta@example.org",
                Phone = "+51 9",
                Password = "secret12345",
                ProfileImg = "img",
                Role = 0,
                CompanyName = "ONG Y",
                CompanyEmail = "contact@ongy.org",
                CompanyCountry = "PE",
                TeamRegisterCode = ""
            };
            
            var mgrResp = await _client.PostAsJsonAsync("/api/v1/Users/sign-up", mgrPayload);
            var raw = await mgrResp.Content.ReadAsStringAsync();
            mgrResp.IsSuccessStatusCode.Should().BeTrue($"Body: {raw}");
            var data = ExtractData(raw);
            var teamCode = GetPropPascalOrCamel(data, "TeamRegisterCode", "teamRegisterCode").GetString();
            teamCode.Should().NotBeNullOrEmpty();
            var companyId = GetPropPascalOrCamel(data, "CompanyId", "companyId").GetInt32();

            // 2) Crear proyecto
            var projectPayload = new
            {
                Name = "Campaña Salud",
                Description = "Jornadas médicas",
                ImageUrl = new[] { "a.png" },
                CompanyId = companyId,
                ProjectDate = DateOnly.FromDateTime(DateTime.Today),
                ProjectTime = TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm"),
                ProjectLocation = "Lima"
            };
            var projResp = await _client.PostAsJsonAsync("/api/v1/Projects", projectPayload);
            projResp.StatusCode.Should().Be(HttpStatusCode.OK);
            var projJson = JsonDocument.Parse(await projResp.Content.ReadAsStringAsync()).RootElement;
            var projectId = projJson.GetProperty("Id").GetInt32();
            
            // 3) Crear un miembro de equipo (TeamMember) usando teamCode
            var memberPayload = new
            {
                FirstName = "Jose",
                LastName = "Chavez",
                Age = 26,
                Email = "jose@example.org",
                Phone = "+51 7",
                Password = "secret12345",
                ProfileImg = "img",
                Role = 1,
                CompanyName = "", CompanyEmail = "", CompanyCountry = "",
                TeamRegisterCode = teamCode
            };
            var memResp = await _client.PostAsJsonAsync("/api/v1/Users/sign-up", memberPayload);
            var memRaw = await memResp.Content.ReadAsStringAsync();
            memResp.IsSuccessStatusCode.Should().BeTrue($"Body: {memRaw}");
            var memData = ExtractData(memRaw);
            teamCode.Should().NotBeNullOrEmpty();
            var name = GetPropPascalOrCamel(memData, "Name", "name").GetString();
            name.Should().NotBeNullOrEmpty();
            var assigneeId = GetPropPascalOrCamel(memData, "Id", "id").GetInt32();
            
        // 4) Crear Tarea (ToDo)
        var createTask = new {
            Title = "Visitar comunidad",
            Description = "Recolectar datos",
            DueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(3).ToString("yyyy-MM-dd"),
            State = "ToDo",
            AssigneeId = assigneeId
        };
        
        var resp4 = await _client.PostAsJsonAsync($"/api/v1/Projects/{projectId}/TaskItems", createTask);
        resp4.StatusCode.Should().Be(HttpStatusCode.OK);
        var taskJson = JsonDocument.Parse(await resp4.Content.ReadAsStringAsync());
        var taskId = taskJson.RootElement.GetProperty("Id").GetInt32();
        taskJson.RootElement.GetProperty("State").GetString().Should().Be("ToDo");
        
        // 5) Listar Tareas
        var resp5 = await _client.GetAsync($"/api/v1/Projects/{projectId}/TaskItems/all");
        resp5.StatusCode.Should().Be(HttpStatusCode.OK);

        }
        
        // helpers
        private static JsonElement ExtractData(string raw)
        {
            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            // Debe ser un objeto { status_code, message, data }
            root.ValueKind.Should().Be(JsonValueKind.Object, $"Body was not JSON object. Body: {raw}");

            var data = root.GetProperty("data");

            // Si data viene como string JSON -> doble parse
            if (data.ValueKind == JsonValueKind.String)
            {
                using var innerDoc = JsonDocument.Parse(data.GetString()!);
                return innerDoc.RootElement.Clone(); // independiente del innerDoc
            }

            // Si ya es objeto, clónalo para que no dependa de doc
            return data.Clone();
        }
        
        private static JsonElement RequireObjectProp(JsonElement root, string name)
        {
            if (!root.TryGetProperty(name, out var value))
                Assert.Fail($"Missing property '{name}'. Body was: {root.GetRawText()}");
            return value;
        }

        private static JsonElement GetPropPascalOrCamel(JsonElement obj, string pascal, string camel)
        {
            if (obj.TryGetProperty(pascal, out var v1)) return v1;
            if (obj.TryGetProperty(camel, out var v2)) return v2;
            Assert.Fail($"Missing property '{pascal}'/'{camel}'. Object: {obj.GetRawText()}");
            return default; // unreachable
        }
    }
}
