namespace MyWebServer.Server.HTTP
{
    using System.Web;

    public class Request
    {
        private static Dictionary<string, Session> Sessions = new();

        public string URL { get; private set; }

        public string Body { get; private set; }

        public Method Method { get; private set; }

        public HeaderCollection Headers { get; private set; }

        public CookieCollection Cookies { get; private set; }

        public Session Session { get; private set; }

        public IReadOnlyDictionary<string, string> Form { get; private set; }

        public IReadOnlyDictionary<string, string> Query { get; private set; }

        public static Request Parse(string request)
        {
            var lines = request.Split("\r\n");

            var firstLine = lines
                .First()
                .Split(" ");

            var method = ParseMethod(firstLine[0]);
            (string url, Dictionary<string, string> query) = ParseUrl(firstLine[1]);

            var headers = ParseHeaders(lines.Skip(1));

            var cookies = ParseCookies(headers);

            var session = GetSession(cookies);

            var bodyLines = lines.Skip(headers.Count + 2).ToArray();

            var body = string.Join("\r\n", bodyLines);

            var form = ParseForm(headers, body);

            return new Request
            {
                Method = method,
                URL = url,
                Headers = headers,
                Cookies = cookies,
                Body = body,
                Session = session,
                Form = form,
                Query = query
            };
        }
        private static (string url, Dictionary<string, string> query) ParseUrl(string queryString)
        {
            string url = String.Empty;
            Dictionary<string, string> query = new Dictionary<string, string>();
            var parts = queryString.Split("?", 2);

            if (parts.Length > 1)
            {
                var queryParams = parts[1].Split("&");

                foreach (var pair in queryParams)
                {
                    var param = pair.Split('=');

                    if (param.Length == 2)
                    {
                        query.Add(param[0], param[1]);
                    }
                }
            }

            url = parts[0];

            return (url, query);
        }

        private static CookieCollection ParseCookies(HeaderCollection headers)
        {
            var cookies = new CookieCollection();

            if (headers.Contains(Header.Cookie))
            {
                var cookieHeader = headers[Header.Cookie];
                var allCookies = cookieHeader.Split(';', StringSplitOptions.RemoveEmptyEntries);

                foreach (var cookie in allCookies)
                {
                    var values = cookie.Split('=', StringSplitOptions.RemoveEmptyEntries);

                    cookies.Add(values[0]?.Trim(), values[1]?.Trim());
                }
            }

            return cookies;
        }

        private static Session GetSession(CookieCollection cookies)
        {
            var sessionId = cookies.Contains(Session.SessionCookieName)
                ? cookies[Session.SessionCookieName]
                : Guid.NewGuid().ToString();

            if (!Sessions.ContainsKey(sessionId))
            {
                Sessions[sessionId] = new Session(sessionId);
            }

            return Sessions[sessionId];
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

        private static Dictionary<string, string> ParseForm
            (HeaderCollection headers, string body)
        {
            var formCollection = new Dictionary<string, string>();

            if (headers.Contains(Header.ContentType)
                && headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {
                var parsedResult = ParseFormData(body);

                foreach (var (name, value) in parsedResult)
                {
                    formCollection.Add(name, value);
                }
            }

            return formCollection;
        }

        private static Dictionary<string, string> ParseFormData(string bodyLines)
            => HttpUtility.UrlDecode(bodyLines)
            .Split('&')
            .Select(part => part.Split('='))
            .Where(part => part.Length == 2)
            .ToDictionary(
                part => part[0],
                part => part[1],
                StringComparer.InvariantCultureIgnoreCase);
    }
}