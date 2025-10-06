using System.Net.Http;
using System.Text.Json;

namespace AidManager.BDD.Support
{
    public class World
    {
        public AidManagerFactory Factory { get; set; } = null!;
        public HttpClient Client { get; set; } = null!;

        // Shared state
        public string TeamRegisterCode { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public int AssigneeId { get; set; }

        public JsonDocument? LastJson { get; set; }
        public HttpResponseMessage? LastResponse { get; set; }
    }
}
