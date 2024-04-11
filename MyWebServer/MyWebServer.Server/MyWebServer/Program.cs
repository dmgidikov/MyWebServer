namespace MyWebServer
{
    using System.Net;
    using System.Text;
    using System.Net.Sockets;

    public class Program
    {
        public static void Main()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var port = 5555;

            var server = new HttpServer(ipAddress, port);
            server.Start();
        }
    }
}