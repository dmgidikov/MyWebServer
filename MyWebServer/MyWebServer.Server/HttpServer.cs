namespace MyWebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;

        public HttpServer(IPAddress _address, int _port)
        {
            ipAddress = _address;
            port = _port;

            serverListener = new TcpListener(ipAddress, port);
        }

        public void Start()
        {            
            serverListener.Start();

            Console.WriteLine($"Server is listening on port {port}");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = serverListener.AcceptTcpClient();
                var networkStream = connection.GetStream();

                var requestText = ReadRequest(networkStream);
                Console.WriteLine(requestText);

                WriteResponse(networkStream, "Hello from the server!");

                connection.Close();
            }
        }

        private void WriteResponse(NetworkStream networkStream, string content)
        {            
            var contentLength = Encoding.UTF8.GetByteCount(content);

            var responseString = $@"HTTP/1.1 200 OK
Content-type: text/plain; charset=UTF-8
Content-Length: {contentLength}

{content}";

            var responseBytes = Encoding.UTF8.GetBytes(responseString);
            networkStream.Write(responseBytes);
        }

        private string ReadRequest(NetworkStream networkStream)
        {
            var bufferLenght = 1024;
            var buffer = new byte[bufferLenght];

            var totalBytes = 0;

            var requestBuilder = new StringBuilder();

            do
            {
                var bytesRead = networkStream.Read(buffer, 0, bufferLenght);

                totalBytes += bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large");
                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            while (networkStream.DataAvailable);

            return requestBuilder.ToString();   
        }
    }
}