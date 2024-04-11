namespace MyWebServer.Server.Responses
{
    using HTTP;

    public class RedirectResponse : Response
    {
        public RedirectResponse(string location)
            : base(StatusCode.Found)
        {
            Headers.Add(Header.Location, location);
        }
    }
}
