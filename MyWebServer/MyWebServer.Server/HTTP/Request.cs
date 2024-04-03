namespace MyWebServer.Server.HTTP
{
    public class Request
    {
        public string URL { get; private set; }

        public string Body { get; private set; }

        public Method Method { get; private set; }

        public HeaderCollection Headers { get; private set; }


        public static Request Parse(string request)
        {
            var lines = request.Split("\r\n");

            var firstLine = lines
                .First()
                .Split(" ");

            var method = ParseMethod(firstLine[0]);
            var url = firstLine[1];
            var headers = ParseHeaders(lines.Skip(1));

            var bodyLines = lines.Skip(headers.Count + 2).ToArray();
            var body = string.Join("\r\n", bodyLines);

            return new Request
            {
                Method = method,
                URL = url,
                Body = body,
                Headers = headers
            };
        }

        private static Method ParseMethod(string method)
        {
            try
            {
                return Enum.Parse<Method>(method);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Method {method} is not supported");
            }
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
        {
            var headerCollection = new HeaderCollection();

            foreach (var headerLine in headerLines)
            {
                if (headerLine == string.Empty)
                {
                    break;
                }

                var headerParts = headerLine.Split(":", 2);

                if (headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is not valid");
                }

                var headerName = headerParts[0];
                var headerValue = headerParts[1];

                headerCollection.Add(headerName, headerValue);

            }

            return headerCollection;
        }
    }
}