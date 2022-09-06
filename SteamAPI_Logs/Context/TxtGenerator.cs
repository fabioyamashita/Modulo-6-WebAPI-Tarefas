using System.Reflection;

namespace SteamAPI.Context
{
    public class TxtGenerator
    {
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
