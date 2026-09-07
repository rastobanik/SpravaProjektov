using System.Text.Json.Serialization;

namespace SpravaProjektovAPI.Application.Projects
{
    public class CustomerResponse
    {
        [JsonPropertyName("customer")]
        public string Customer { get; set; } = string.Empty;
    }
}
