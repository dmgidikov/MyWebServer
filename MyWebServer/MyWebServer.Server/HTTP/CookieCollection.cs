namespace MyWebServer.Server.HTTP
{
    using System.Collections;

    public class CookieCollection : IEnumerable<Cookie>
    {
        private readonly Dictionary<string, Cookie> cookies = new Dictionary<string, Cookie>();

        public string this[string name]
            => cookies[name].Value;

        public void Add(string name, string value)
             => cookies[name] = new Cookie(name, value);

        public bool Contains(string name)
            => cookies.ContainsKey(name);       

        public IEnumerator<Cookie> GetEnumerator()
            => cookies.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
} 