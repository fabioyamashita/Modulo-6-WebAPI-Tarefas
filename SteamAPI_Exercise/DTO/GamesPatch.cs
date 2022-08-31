using System.Text.Json.Serialization;

namespace SteamAPI.DTO
{
    public class GamesPatch
    {
        public string Platforms { get; set; }
        public string Categories { get; set; }
        public string Genres { get; set; }
    }
}
