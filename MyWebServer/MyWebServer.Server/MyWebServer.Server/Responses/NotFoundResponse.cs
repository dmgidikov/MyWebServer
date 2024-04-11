namespace MyWebServer.Server.Responses
{
    using HTTP;

    public class NotFoundResponse : Response
    {
        public NotFoundResponse()
            : base(StatusCode.NotFound)
        {
        }
    }
}
