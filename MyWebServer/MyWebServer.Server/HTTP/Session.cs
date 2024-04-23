namespace MyWebServer.Server.HTTP
{
    using Common;

    public class Session
    {
        public const string SessionCookieName = "MyWebServerSID";
        public const string SessionCurrentDateKey = "CurrentDate";
        public const string SessionUserKey = "AuthenticatedUserId";

        private readonly Dictionary<string, string> sessions;

        public Session(string id)
        {
            Guard.AgainsNull(id, nameof(id));

            Id = id;
            sessions = new Dictionary<string, string>();
        }

        public string Id { get; init; }

        public string this[string key]
        {
            get => sessions[key];
            set => sessions[key] = value;
        }

        public bool ContainsKey(string name)
            => sessions.ContainsKey(name);

        public void Clear()
            => sessions.Clear();
    }
}