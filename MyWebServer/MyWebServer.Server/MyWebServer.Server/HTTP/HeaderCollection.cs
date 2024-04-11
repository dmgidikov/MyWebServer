namespace MyWebServer.Server.HTTP
{
    using System.Collections;

    public class HeaderCollection : IEnumerable<Header>
    {
        private readonly Dictionary<string, Header> headers = new Dictionary<string, Header>();

        public string this[string name]
            => this.headers[name].Value;

        public int Count => headers.Count;

        public bool Contains(string name)
            => headers.ContainsKey(name);

        public void Add(string name, string value)
            => headers[name] = new Header(name, value);

        public IEnumerator<Header> GetEnumerator()
            => headers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}