namespace MyWebServer.Server.Responses
{
    using HTTP;

    public class HtmlResponse : ContentResponse
    {
        public HtmlResponse(string text, Action<Request, Response> prerender = null)
            : base(text, ContentType.Html, prerender)
        {
        }
    }
}