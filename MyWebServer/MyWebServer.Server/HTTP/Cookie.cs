namespace MyWebServer.Server.HTTP
{
    using Common;
    using System.Text;

    public class Cookie
    {
        private const int CookieDefaultExpirationDays = 3;
        private const string CookieDefaultPath = "/";

        public Cookie(string name, string value,
            int expires = CookieDefaultExpirationDays, string path = CookieDefaultPath)
        {
            Guard.AgainsNull(name, nameof(name));
            Guard.AgainsNull(value, nameof(value));

            Name = name;
            Value = value;
            IsNew = true;
            Path = path;
            Expires = DateTime.UtcNow.AddDays(expires);
        }

        public Cookie(string name, string value, bool isNew,
            int expires = CookieDefaultExpirationDays, string path = CookieDefaultPath)
            : this(name, value, expires, path)
        {
            IsNew = isNew;
        }


        public string Name { get; set; }

        public string Value { get; set; }

        public DateTime Expires { get; set; }

        public string Path { get; set; }

        public bool IsNew { get; set; }

        public bool HttpOnly { get; set; }


        public void Delete()
        {
            Expires = DateTime.UtcNow.AddDays(-1);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"{Name}={Value}: Expires={Expires}");

            if (HttpOnly)
            {
                sb.Append("; HttpOnly");
            }

            sb.Append($"; Path={Path}");

            return sb.ToString();
        }
    }
}