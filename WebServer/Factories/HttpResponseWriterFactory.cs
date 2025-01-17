using System.Net.Sockets;
using WebServer.Domain.Core.Response;
using WebServer.Domain.Interfaces.Factories;
using WebServer.Domain.Interfaces.Server;
using WebServer.Tasks;

namespace WebServer.Factories;

public class HttpResponseWriterFactory : IHttpResponseWriterFactory
{
    public IResponseWriter Create(Socket socket, HttpResponse response)
    {
        lock (this)
        {
            return new HttpResponseWriter(socket, response);
        }
    }
}