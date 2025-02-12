using System.Net.Sockets;
using WebServer.Domain.Core.Response;
using WebServer.Domain.Interfaces.Server;

namespace WebServer.Domain.Interfaces.Factories;

public interface IHttpResponseWriterFactory
{
    IResponseWriter Create(Socket socket, HttpResponse response);
}