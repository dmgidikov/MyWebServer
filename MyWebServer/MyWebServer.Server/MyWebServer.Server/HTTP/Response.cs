namespace MyWebServer.Server.HTTP
{
    using System.Text;

    public class Response
    {
        public Response(StatusCode statusCode) 
        {
            StatusCode = statusCode;

            Headers.Add(Header.Server, "SoftUni Server");
            Headers.Add(Header.Date, $"{DateTime.UtcNow:r}");
        }

        public StatusCode StatusCode { get; init; }

        public HeaderCollection Headers { get; } = new HeaderCollection();

        public string Body{ get; set; }

        public Action<Request, Response> PreRenderAction { get; protected set; }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"HTTP/1.1 {(int)StatusCode} {StatusCode}");

            foreach (var header in Headers)
            {
                result.AppendLine(header.ToString());
            }

            result.AppendLine();

            if (string.IsNullOrEmpty(Body) == false)
            {
                result.Append(Body);
            }

            return result.ToString();
        }
    }
}