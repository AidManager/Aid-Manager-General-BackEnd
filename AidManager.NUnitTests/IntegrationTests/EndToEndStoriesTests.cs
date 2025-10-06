using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AidManager.NUnitTests.IntegrationTests
{
    [TestFixture]
    public class EndToEndStoriesTests
    {
        private AidManagerFactory _factory = null!;
        private HttpClient _client = null!;

        [SetUp]
        public void Setup()
        {
            _factory = new AidManagerFactory();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void Cleanup()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [Test]
        public async Task Test_Create_Manager_and_Register_Collaborator_By_Code()
        {
            // HU04/HU06: Sign-up Manager crear company y teamcode
            var managerPayload = new
            {
                FirstName = "Ana",
                LastName = "Pérez",
                Age = 28,
                Email = "ana.manager@example.org",
                Phone = "+51 1",
                Password = "secret1234",
                ProfileImg = "img.png",
                Role = 0, // Manager
                CompanyName = "ONG X",
                CompanyEmail = "ong@example.org",
                CompanyCountry = "PE",
                TeamRegisterCode = "" 
            };
            var mgrResp = await _client.PostAsJsonAsync("/api/v1/Users/sign-up", managerPayload);
            var raw = await mgrResp.Content.ReadAsStringAsync();
            mgrResp.IsSuccessStatusCode.Should().BeTrue($"Body: {raw}");
            var data = ExtractData(raw);
            var teamCode = GetPropPascalOrCamel(data, "TeamRegisterCode", "teamRegisterCode").GetString();
            teamCode.Should().NotBeNullOrEmpty();

            Console.WriteLine(teamCode);

            
            // HU07/HU08: Sign-up TeamMember usando el teamcode previamente creado
            var memberPayload = new
            {
                FirstName = "Luis",
                LastName = "Rojas",
                Age = 31,
                Email = "luis.tm@example.org",
                Phone = "+51 2",
                Password = "secret12345",
                ProfileImg = "pic.png",
                Role = 1, // TeamMember
                CompanyName = "", CompanyEmail = "", CompanyCountry = "",
                TeamRegisterCode = teamCode
            };
            var memResp = await _client.PostAsJsonAsync("/api/v1/Users/sign-up", memberPayload);
            var raw2 = await memResp.Content.ReadAsStringAsync();
            memResp.IsSuccessStatusCode.Should().BeTrue($"Body: {raw2}");
        }

        [Test]
        public async Task Test_Projects_And_Tasks_Flow()
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
        
            // 2) Crear un miembro de equipo (TeamMember) usando teamCode
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
        
        
            // HU10: Crear proyecto
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
            // projResp.StatusCode.Should().Be(HttpStatusCode.OK);
            // var projJson = JsonDocument.Parse(await projResp.Content.ReadAsStringAsync()).RootElement;
            // var projectId = projJson.GetProperty("Id").GetInt32();
            
            // var projRaw = await projResp.Content.ReadAsStringAsync();
            // // projResp.IsSuccessStatusCode.Should().BeTrue(projRaw);
            // var projData = ExtractData(projRaw);
            // var projectId = GetPropPascalOrCamel(projData, "Id", "id").GetInt32();
            
        
            // HU13: Create Task under project
            // var taskPayload = new
            // {
            //     Title = "Visitar comunidad",
            //     Description = "Recolectar datos",
            //     DueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(3),
            //     State = "ToDo",
            //     AssigneeId = assigneeId
            // };
            // var taskResp = await _client.PostAsJsonAsync($"/api/v1/Projects/{projectId}/TaskItems", taskPayload);
            // taskResp.StatusCode.Should().Be(HttpStatusCode.OK);
            // var taskJson = JsonDocument.Parse(await taskResp.Content.ReadAsStringAsync()).RootElement;
            // var taskId = taskJson.GetProperty("Id").GetInt32();
            // taskJson.GetProperty("State").GetString().Should().Be("ToDo");
            
            // var taskRaw = await taskResp.Content.ReadAsStringAsync();
            // taskResp.IsSuccessStatusCode.Should().BeTrue($"Body: {projRaw}");
            // var taskData = ExtractData(taskRaw);
            // var taskId = GetPropPascalOrCamel(taskData, "Id", "id").GetInt32();
        
            // // HU12: List tasks by project
            // var listResp = await _client.GetAsync($"/api/v1/Projects/{projectId}/TaskItems/all");
            // listResp.StatusCode.Should().Be(HttpStatusCode.OK);
            //
            // // HU14: Update state
            // var updatePayload = new
            // {
            //     Title = "Visitar comunidad",
            //     Description = "Recolectar datos",
            //     DueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(4),
            //     State = "Done",
            //     AssigneeId = assigneeId
            // };
            // var upResp = await _client.PutAsJsonAsync($"/api/v1/Projects/{projectId}/TaskItems/edit/{taskId}", updatePayload);
            // upResp.StatusCode.Should().Be(HttpStatusCode.OK);
            // var upJson = JsonDocument.Parse(await upResp.Content.ReadAsStringAsync()).RootElement;
            // upJson.GetProperty("State").GetString().Should().Be("Done");
            //
            // // HU15: Delete task
            // var delResp = await _client.DeleteAsync($"/api/v1/Projects/{projectId}/TaskItems/{taskId}");
            // delResp.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        // [Test]
        // public async Task Test_Events_And_Analytics_Flow()
        // {
        //     // Create manager to have a company and then a project to link analytics/events
        //     var mgrPayload = new
        //     {
        //         FirstName = "Eva",
        //         LastName = "Gomez",
        //         Age = 32,
        //         Email = "eva@example.org",
        //         Phone = "+51 5",
        //         Password = "secret12345",
        //         ProfileImg = "img",
        //         Role = 0,
        //         CompanyName = "ONG Z",
        //         CompanyEmail = "ongz@example.org",
        //         CompanyCountry = "PE",
        //         TeamRegisterCode = ""
        //     };
        //     var mgrResp = await _client.PostAsJsonAsync("/api/v1/Users/sign-up", mgrPayload);
        //     var raw = await mgrResp.Content.ReadAsStringAsync();
        //     mgrResp.IsSuccessStatusCode.Should().BeTrue($"Body: {raw}");
        //
        //     // var data = ExtractData(raw);
        //     // var companyId = GetPropPascalOrCamel(data, "CompanyId", "companyId").GetInt32();
        //     var companyId = 0;
        //     
        //     using (var doc = JsonDocument.Parse(raw))
        //     {
        //         var root = doc.RootElement;
        //         var data = root.GetProperty("data");
        //         var tempCompanyId = (data.TryGetProperty("CompanyId", out var c1) ? c1 : data.GetProperty("companyId")).GetInt32();
        //         companyId = tempCompanyId;
        //     }
        //
        //     // 2) Login y setear Authorization: Bearer <token>
        //     await SignInAndAttachBearerAsync("eva@example.org", "secret12345");
        //     
        //     // Create a project
        //     var projectPayload = new
        //     {
        //         Name = "Proyecto Z",
        //         Description = "Desc",
        //         ImageUrl = Array.Empty<string>(),
        //         CompanyId = companyId,
        //         ProjectDate = DateOnly.FromDateTime(DateTime.Today),
        //         ProjectTime = TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm"),
        //         ProjectLocation = "Cusco"
        //     };
        //     var projResp = await _client.PostAsJsonAsync("/api/v1/Projects", projectPayload);
        //     var projRaw = await projResp.Content.ReadAsStringAsync();
        //     projResp.IsSuccessStatusCode.Should().BeTrue($"Body: {projRaw}");
        //     var projData = ExtractData(projRaw);
        //     var projectId = GetPropPascalOrCamel(projData, "Id", "id").GetInt32();
        //
        //     // HU16: Create Event (dates)
        //     var eventPayload = new
        //     {
        //         Title = "Reunión",
        //         Description = "Detalle",
        //         EventDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
        //         EventTime = "14:00",
        //         Location = "Cusco",
        //         ProjectId = projectId
        //     };
        //     var evResp = await _client.PostAsJsonAsync("/api/v1/Events", eventPayload);
        //     evResp.StatusCode.Should().Be(HttpStatusCode.OK);
        //
        //     // HU17: Create Analytics and update
        //     var analyticsCreate = new
        //     {
        //         ProjectId = projectId,
        //         Status = "OnTrack",
        //         TasksDone = 5,
        //         TasksTotal = 10,
        //         Progressbar = 0.5
        //     };
        //     var anResp = await _client.PostAsJsonAsync("/api/v1/Analytics", analyticsCreate);
        //     anResp.StatusCode.Should().Be(HttpStatusCode.OK);
        // }
        
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
        
        private async Task<string> SignInAndAttachBearerAsync(string email, string password)
        {
            var resp = await _client.PostAsJsonAsync("/api/v1/Authentication/sign-in", new
            {
                Email = email,
                Password = password
            });
            var raw = await resp.Content.ReadAsStringAsync();
            resp.IsSuccessStatusCode.Should().BeTrue($"SignIn failed: {raw}");

            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            // token puede venir como "Token" o "token"
            var tokenProp = root.TryGetProperty("Token", out var t1) ? t1
                : root.TryGetProperty("token", out var t2) ? t2
                : throw new Exception($"Token not found in sign-in response: {raw}");

            var token = tokenProp.GetString();
            token.Should().NotBeNullOrEmpty($"Empty token in sign-in response: {raw}");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return token!;
        }

    }
}
