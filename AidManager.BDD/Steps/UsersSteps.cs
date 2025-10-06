using System.Text.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AidManager.BDD.Steps
{
    [Binding]
    public class UsersSteps
    {
        private readonly Support.World _world;
        public UsersSteps(Support.World world) => _world = world;

        [When(@"registro un Manager con datos válidos")]
        public async Task WhenRegistroManager()
        {
            var payload = new {
                FirstName = "Ana",
                LastName = "Pérez",
                Age = 28,
                Email = "ana.manager@example.org",
                Phone = "+51 1",
                Password = "Secr3t!",
                ProfileImg = "img.png",
                Role = 0,
                CompanyName = "ONG X",
                CompanyEmail = "ong@example.org",
                CompanyCountry = "PE",
                TeamRegisterCode = ""
            };
            _world.LastResponse = await _world.Client.PostAsJsonAsync("/api/v1/Users/sign-up", payload);
            // var json = JsonDocument.Parse(await _world.LastResponse.Content.ReadAsStringAsync());
            var raw = await _world.LastResponse.Content.ReadAsStringAsync();
            _world.LastResponse.IsSuccessStatusCode.Should().BeTrue($"Body: {raw}");
            var data = ExtractData(raw);
            var teamCode = GetPropPascalOrCamel(data, "TeamRegisterCode", "teamRegisterCode").GetString();
            teamCode.Should().NotBeNullOrEmpty();
            var companyId = GetPropPascalOrCamel(data, "CompanyId", "companyId").GetInt32();
            

            // var data = json.RootElement.GetProperty("data");
            _world.TeamRegisterCode = teamCode;
            _world.CompanyId = companyId;
        }

        [Then(@"la respuesta es 200 OK")]
        public void ThenRespuesta200()
        {
            _world.LastResponse!.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Then(@"obtengo un TeamRegisterCode no vacío")]
        public void ThenCodeNoVacio()
        {
            _world.TeamRegisterCode.Should().NotBeNullOrWhiteSpace();
        }
        
        [Then(@"guardo el CompanyId")]
        public void ThenGuardoCompanyId()
        {
            _world.CompanyId.Should().BeGreaterThan(0);
        }
        
        [Given(@"ya registré un Manager y tengo su TeamRegisterCode")]
        public async Task GivenTengoTeamCode()
        {
            await WhenRegistroManager();
        }
        
        [When(@"registro un TeamMember usando ese TeamRegisterCode")]
        public async Task WhenRegistroTeamMember()
        {
            var payload = new {
                FirstName = "Luis",
                LastName = "Rojas",
                Age = 31,
                Email = "luis.tm@example.org",
                Phone = "+51 2",
                Password = "x",
                ProfileImg = "pic.png",
                Role = 1,
                CompanyName = "",
                CompanyEmail = "",
                CompanyCountry = "",
                TeamRegisterCode = _world.TeamRegisterCode
            };
            _world.LastResponse = await _world.Client.PostAsJsonAsync("/api/v1/Users/sign-up", payload);
            var json = JsonDocument.Parse(await _world.LastResponse.Content.ReadAsStringAsync());
            _world.LastJson = json;
        }
        
        [Then(@"el TeamMember tiene un Id asignado")]
        public void ThenTeamMemberIdAsignado()
        {
            var id = _world.LastJson!.RootElement.GetProperty("data").GetProperty("Id").GetInt32();
            id.Should().BeGreaterThan(0);
            _world.AssigneeId = id;
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
        
        private static JsonElement GetPropPascalOrCamel(JsonElement obj, string pascal, string camel)
        {
            if (obj.TryGetProperty(pascal, out var v1)) return v1;
            if (obj.TryGetProperty(camel, out var v2)) return v2;
            Assert.Fail($"Missing property '{pascal}'/'{camel}'. Object: {obj.GetRawText()}");
            return default; // unreachable
        }
    }
}
