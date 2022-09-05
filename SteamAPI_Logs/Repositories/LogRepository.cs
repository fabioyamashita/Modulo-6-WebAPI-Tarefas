using System.Reflection;
using SteamAPI.Interfaces;

namespace SteamAPI.Repositories
{
    public class LogRepository : ILogRepository
    {
        public Task Insert(string message)
        {
            return Task.Run(() =>
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string fileName = "steamData.txt";

                using StreamWriter file = new($@"{path}\{fileName}", append: true);
                file.WriteLine(message);
            });
        }
    }
}
