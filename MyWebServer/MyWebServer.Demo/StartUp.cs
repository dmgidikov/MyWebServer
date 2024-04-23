namespace MyWebServer.Demo
{
    using System;
    using System.Net;
    using System.Text;
    using System.Web;
    using Server.HTTP;
    using Server.Responses;

    public class StartUp
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
   Name: <input type='text' name='Name'/>
   Age: <input type='number' name ='Age'/>
<input type='submit' value ='Save' />
</form>";

        private const string DownloadForm = @"<form action='/Content' method='POST'>
   <input type='submit' value ='Download Sites Content' /> 
</form>";

        private static string FileName = "content.txt";

        private const string LoginForm = @"<form action='/Login' method='POST'>
Username: <input type='text' name='Username'/>
Password: <input type='text' name='Password'/>
<input type='submit' value ='Log In' /
</form>";

        private const string Username = "user";

        private const string Password = "user123";

        public static async Task Main()
        {
            await DownloadSiteAsTextFile(FileName,
            [
                "https://judge.softuni.org/",
                "https://softuni.org/"]
            );

            var server = new HttpServer(routes => routes
            .MapGet("/", new TextResponse("Hello from server!"))
            .MapGet("/Redirect", new RedirectResponse("https://softuni.org/"))
            .MapGet("/HTML", new HtmlResponse(HtmlForm))
            .MapPost("/HTML", new TextResponse("", AddFormDataAction))
            .MapGet("/Content", new HtmlResponse(DownloadForm))
            .MapPost("/Content", new TextFileResponse(FileName))
            .MapGet("/Cookies", new HtmlResponse("", AddCookieAction))
            .MapGet("/Session", new TextResponse("", DisplaySessionInfoAction))
            .MapGet("/Login", new HtmlResponse(LoginForm))
            .MapPost("/Login", new HtmlResponse("", LoginAction))
            .MapGet("/Logout", new HtmlResponse("", LogoutAction))
            .MapGet("/UserProfile", new HtmlResponse("", GetUserDataAction)));

            await server.Start();
        }

        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = "";

            foreach (var (key, value) in request.Form)
            {
                response.Body += $"{key} - {value}";
                response.Body += Environment.NewLine;
            }
        }

        private static async Task DownloadSiteAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }

            var response = await Task.WhenAll(downloads);

            var responseString = string.Join(Environment.NewLine + new string('-', 100), response);

            await File.WriteAllTextAsync(fileName, responseString);
        }

        private static async Task<string> DownloadWebSiteContent(string url)
        {
            var httpClient = new HttpClient();

            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);

                var html = await response.Content.ReadAsStringAsync();

                return html.Substring(0, 2000);
            }
        }

        private static void AddCookieAction(Request request, Response response)
        {
            var requestHasCookies = request.Cookies.Any();
            string? body;

            if (requestHasCookies)
            {
                var sb = new StringBuilder();
                sb.AppendLine("<h1>Cookies</h1>");

                foreach (var item in request.Cookies)
                {
                    sb.AppendLine($"{HttpUtility.HtmlEncode(item.Name)}:{HttpUtility.HtmlEncode(item.Value)}");
                }

                body = sb.ToString();
            }
            else
            {
                body = "<h1>Cookie set!</h1>";

                response.Cookies.Add("My-Cookie", "SecretValue");
                response.Cookies.Add("My-Cookie2", "GoodValue");
            }

            response.Body = body;
        }

        private static void DisplaySessionInfoAction(Request request, Response response)
        {
            var sessionExits = request.Session.ContainsKey(Session.SessionCurrentDateKey);

            var bodyText = "";

            if (sessionExits)
            {
                var currentDate = request.Session[Session.SessionCurrentDateKey];
                bodyText = $"Stored date: {currentDate}";
            }
            else
            {
                bodyText = "Current date stored!";
            }

            response.Body = "";
            response.Body += bodyText;
        }

        private static void LoginAction(Request request, Response response)
        {
            request.Session.Clear();

            var bodyText = "";

            var usernameMatches = request.Form["Username"] == Username;
            var passwordMatches = request.Form["Password"] == Password;

            if (usernameMatches && passwordMatches)
            {
                request.Session[Session.SessionUserKey] = "MyUserId";
                response.Cookies.Add(Session.SessionCookieName, request.Session.Id);

                bodyText = "<h3>Logged successfully!</h3>";
            }
            else
            {
                bodyText = LoginForm;
            }

            response.Body = "";
            response.Body += bodyText;
        }

        private static void LogoutAction(Request request, Response response)
        {
            request.Session.Clear();

            response.Body = "";
            response.Body += "<h3>Logged out successfully!</h3>";
        }

        private static void GetUserDataAction(Request request, Response response)
        {
            if (request.Session.ContainsKey(Session.SessionUserKey))
            {
                response.Body = "";
                response.Body += $"<h3>Currently logged-in user is with the username '{Username}'</h3>";
            }
            else
            {
                response.Body = "";
                response.Body += "<h3>You should first log in - <a href='/Login'>Login</a></h3>";
            }
        }
    }
} 