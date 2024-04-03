namespace MyWebServer.Server.HTTP
{
    public class Response
    {
        public Response(StatusCode statusCode) 
        {
            StatusCode = statusCode;

            Headers.Add("Server", "SoftUni Server");
            Headers.Add("Date", $"{DateTime.UtcNow:r}");
        }

        public StatusCode StatusCode { get; init; }

        public HeaderCollection Headers { get; } = new HeaderCollection();

        public string Body{ get; set; }
    }
}