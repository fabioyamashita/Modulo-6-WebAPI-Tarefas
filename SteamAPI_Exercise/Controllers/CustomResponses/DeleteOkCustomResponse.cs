namespace SteamAPI.Controllers.CustomResponses
{
    public class DeleteOkCustomResponse
    {
        public string Code { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }

        public DeleteOkCustomResponse()
        {

        }

        public DeleteOkCustomResponse(string code, int id, string message)
        {
            Code = code;
            Id = id;
            Message = message;
        }
    }
}
