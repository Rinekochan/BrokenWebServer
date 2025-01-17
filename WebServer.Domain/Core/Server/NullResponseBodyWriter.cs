using WebServer.Domain.Interfaces.Server;

namespace WebServer.Tasks;

public class NullResponseBodyWriter : IResponseBodyWriter
{
    private NullResponseBodyWriter() { }
    
    public Task WriteAsync(Stream stream, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public static readonly NullResponseBodyWriter Instance = new();
}