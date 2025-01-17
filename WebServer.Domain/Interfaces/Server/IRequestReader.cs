using System.Net.Sockets;
using WebServer.Domain.Core.Request;

namespace WebServer.Domain.Interfaces.Server;

public interface IRequestReader
{
    Task<HttpRequest> ReadRequestAsync(CancellationToken cancellationToken);
}