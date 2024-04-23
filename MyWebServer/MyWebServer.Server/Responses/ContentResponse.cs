namespace MyWebServer.Server.Responses
{
    using HTTP;
    using Common;
    using System.Text;

    public class ContentResponse : Response
    {
        public ContentResponse(string content, string contentType,
            Action<Request, Response> preRenderAction = null)
            : base(StatusCode.OK)
        {
            Guard.AgainsNull(content);
            Guard.AgainsNull(contentType);

            PreRenderAction = preRenderAction;

            Headers.Add(Header.ContentType, contentType);

            Body = content;
        }

        public override string ToString()
        {
            if (Body != null)
            {
                var contentLength = Encoding.UTF8.GetByteCount(Body).ToString();
                Headers.Add(Header.ContentLength, contentLength);
            }

            return base.ToString();
        }
    }
}