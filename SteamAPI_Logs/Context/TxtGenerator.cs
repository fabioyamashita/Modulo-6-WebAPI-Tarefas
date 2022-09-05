using System.Reflection;

namespace SteamAPI.Context
{
    public class TxtGenerator
    {
        private readonly LogContext _logContext;

        public TxtGenerator(LogContext logContext)
        {
            _logContext = logContext;
        }

        public static void GenerateFile()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileName = "steamData.txt";

            if (!File.Exists($@"{path}\{fileName}"))
            {
                File.CreateText($@"{path}\{fileName}");
            }
        }

    }
}
