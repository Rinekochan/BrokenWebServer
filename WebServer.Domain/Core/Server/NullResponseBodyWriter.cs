using WebServer.Domain.Interfaces.Server;

namespace WebServer.Domain.Core.Server;

public class NullResponseBodyWriter : IResponseBodyWriter
{
    private NullResponseBodyWriter() { }
    
    public Task WriteAsync(Stream stream, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public static readonly NullResponseBodyWriter Instance = new();
}