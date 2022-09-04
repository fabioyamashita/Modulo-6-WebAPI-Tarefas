using SteamAPI.Models;

namespace SteamAPI.Dto
{
    public class GamesLoggerDto
    {
        public DateTime Date { get; set; }
        public Games Games { get; set; }
        public string Description { get; set; }
    }
}
