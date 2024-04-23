namespace MyWebServer.Server.HTTP
{
    using Common;

    public class Header
    {
        public const string ContentType = "Content-Type";
        public const string ContentLength = "Content-Length";
        public const string ContentDisposition = "Content-Disposition";
        public const string Cookie = "Cookie";
        public const string Date = "Date";
        public const string Location = "Location";
        public const string Server = "Server";
        public const string SetCookie = "Set-Cookie";

        public Header(string _name, string _value) 
        {
            Guard.AgainsNull(_name, nameof(_name));
            Guard.AgainsNull(_value, nameof(_value));

            this.Name = _name;
            this.Value = _value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
            => $"{Name} {Value}";
    }
}