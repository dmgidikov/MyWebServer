namespace MyWebServer.Server.Routing
{
    using HTTP;
    using Common;
    using Responses;
    using System.Runtime.CompilerServices;

    public class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<Method, Dictionary<string, Response>> routes;

        public RoutingTable() => routes = new()
        {
            [Method.GET] = new(),
            [Method.POST] = new(),
            [Method.PUT] = new(),
            [Method.DELETE] = new()
        };

        public IRoutingTable Map(string url, Method method, Response response)
            => method switch
            {
                Method.GET => MapGet(url, response),
                Method.POST => MapPost(url, response),
                _ => throw new InvalidOperationException($"Method {method} is not supported!")
            };

        public IRoutingTable MapGet(string url, Response response)
        {
            Guard.AgainsNull(url, nameof(url));
            Guard.AgainsNull(response, nameof(response));

            routes[Method.GET][url] = response;

            return this;
         }

        public IRoutingTable MapPost(string url, Response response)
        {
            Guard.AgainsNull(url, nameof(url));
            Guard.AgainsNull(response, nameof(response));

            routes[Method.POST][url] = response;

            return this;
        }

        public Response MatchRequest(Request request)
        {
            var requestMethod = request.Method;
            var requestUrl = request.URL;

            if (routes.ContainsKey(requestMethod) == false
                || routes[requestMethod].ContainsKey(requestUrl) == false)
            {
                return new NotFoundResponse();
            }

            return routes[requestMethod][requestUrl];
        }
    }
}