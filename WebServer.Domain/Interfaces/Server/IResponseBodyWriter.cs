namespace WebServer.Domain.Interfaces.Server;

public interface IResponseBodyWriter
{
    Task WriteAsync(Stream stream, CancellationToken cancellationToken);
}