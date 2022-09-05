namespace SteamAPI.Interfaces
{
    public interface ILogRepository
    {
        Task Insert(string message);
    }
}