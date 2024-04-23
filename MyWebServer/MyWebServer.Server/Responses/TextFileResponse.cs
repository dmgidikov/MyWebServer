namespace MyWebServer.Server.Responses
{
    using HTTP;

    public class TextFileResponse : Response
    {
        public string FileName { get; init; }

        public TextFileResponse(string fileName)
            : base(StatusCode.OK)
        {
            FileName = fileName;

            Headers.Add(Header.ContentType, ContentType.PlainText);
        }

        public override string ToString()
        {
            if (File.Exists(FileName))
            {
                Body = File.ReadAllTextAsync(FileName).Result;

                var fileBytesCount = new FileInfo(FileName).Length;
                Headers.Add(Header.ContentType, fileBytesCount.ToString());

                Headers.Add(Header.ContentDisposition, 
                    $"attachment; filename=\"{FileName}\"");
            }

            return base.ToString();
        }
    }
}