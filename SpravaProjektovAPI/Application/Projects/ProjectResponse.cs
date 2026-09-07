using System.Text.Json.Serialization;

namespace SpravaProjektovAPI.Application.Projects
{
    public class ProjectResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string? Name { get; set; } = string.Empty;

        [JsonPropertyName("abbreviation")]
        public string? Abbreviation { get; set; } = string.Empty;

        [JsonPropertyName("customer")]
        public string? Customer { get; set; } = string.Empty;
    }
}
