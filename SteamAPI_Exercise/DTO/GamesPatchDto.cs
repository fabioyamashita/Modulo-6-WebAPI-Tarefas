using System.Text.Json.Serialization;

namespace SteamAPI.DTO
{
    public class GamesPatchDto
    {
        [JsonPropertyName("platforms")]
        public string Platforms { get; set; }
    }
}
