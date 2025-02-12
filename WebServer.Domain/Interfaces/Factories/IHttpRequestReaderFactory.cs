using System.Net.Sockets;
using WebServer.Domain.Interfaces.Server;

namespace WebServer.Domain.Interfaces.Factories;

public interface IHttpRequestReaderFactory
{
    IRequestReader Create(Socket socket);
}