using System.Net.Sockets;

namespace WebServer.Domain.Interfaces.Server;

public interface IResponseWriter
{
    Task WriteAsync(CancellationToken token);
}