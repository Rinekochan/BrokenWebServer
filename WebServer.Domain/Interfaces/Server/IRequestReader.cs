using System.Net.Sockets;
using WebServer.Domain.Core.Request;

namespace WebServer.Domain.Interfaces.Server;

public interface IRequestReader
{
    Task<HttpRequest> ReadRequestAsync(Socket socket, CancellationToken cancellationToken);
}