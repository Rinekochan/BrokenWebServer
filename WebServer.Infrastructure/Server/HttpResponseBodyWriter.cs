using WebServer.Domain.Interfaces.Server;

namespace WebServer.Infrastructure.Server;

public class HttpResponseBodyWriter : IResponseBodyWriter
{
    private string _content;
    public HttpResponseBodyWriter(string content)
    {
        _content = content;
    }

    public async Task WriteAsync(Stream stream, CancellationToken cancellationToken)
    {
        StreamWriter writer = new(stream);

        await writer.WriteAsync(_content);

        await writer.FlushAsync(cancellationToken);
    }
}