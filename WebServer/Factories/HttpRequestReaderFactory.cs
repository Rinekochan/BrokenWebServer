using System.Net.Sockets;
using WebServer.Domain.Interfaces.Factories;
using WebServer.Domain.Interfaces.Server;
using WebServer.Tasks;

namespace WebServer.Factories;

public class HttpRequestReaderFactory(ILogger<HttpRequestReader> logger) : IHttpRequestReaderFactory
{
    public IRequestReader Create(Socket socket)
    {
        lock (this)
        {
            return new HttpRequestReader(socket, logger);
        }
    }
}