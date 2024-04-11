namespace MyWebServer.Server.HTTP
{
    public class TextFileResponse : Response
    {
        public string FileName { get; init; }

        public TextFileResponse(string fileName)
            :base(StatusCode.OK)
        {
            FileName = fileName;

            Headers.Add(Header.ContentType, ContentType.PlainText);
        }

        public override string ToString()
        {
            if (File.Exists(FileName))
            {
                this.Body = File.ReadAllTextAsync(FileName).Result;

                var fileBytesCount = new FileInfo(FileName).Length;
                this.Headers.Add(Header.ContentType, fileBytesCount.ToString());

                Headers.Add(Header.ContentDisposition, $"attachment, filename=\"{FileName}\"");
            }

            return base.ToString();
        }
    }
}
