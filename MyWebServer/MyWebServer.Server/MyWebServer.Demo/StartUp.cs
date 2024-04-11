namespace MyWebServer.Demo
{
    using System;
    using System.Net;
    using Server.HTTP;
    using Server.Responses;

    public class StartUp
    {
        private const string HtmlForm = @"<form action=""/HTML"" method=""POST"">
      Name: <input type=""text"" name=""Name"" /> Age:
      <input type=""number"" name=""Age"" />
      <input type=""submit"" value=""Save"" />
    </form>";

        private const string DownloadForm = @"<form action='/Content' method='POST'>
   <input type='submit' value ='Download Sites Content' /> 
</form>";

        private static string FileName = "content.txt";

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

        private static async Task DownloadSiteAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }

            var response = await Task.WhenAll(downloads);

            var responseString = string.Join(Environment.NewLine + new string('-', 100), response);

            await File.WriteAllTextAsync(FileName, responseString);
        }


        public static async Task Main()
        {
            await DownloadSiteAsTextFile(StartUp.FileName, new string[]
            { "https://judge.softuni.org/",
              "https://softuni.org/"});

            var server = new HttpServer(routes => routes
            .MapGet("/", new TextResponse("Hello from server!"))
            .MapGet("/Redirect", new RedirectResponse("https://softuni.org/"))
            .MapGet("/HTML", new HtmlResponse(StartUp.HtmlForm))
            .MapPost("/HTML", new TextResponse("", StartUp.AddFormDataAction))
            .MapGet("/Content", new HtmlResponse(StartUp.DownloadForm))
            .MapPost("/Content", new TextFileResponse(StartUp.FileName)));

            await server.Start();
        }

        private static void AddFormDataAction(Request request, Response response)
        {
            foreach (var (key, value) in request.Form)
            {
                response.Body += $"{key} - {value}";
                response.Body += Environment.NewLine;
            }
        }
    }
}