using WebServer.Domain.Interfaces.Server;

namespace WebServer.Infrastructure.Server;

public class DefaultHttpResponseBodyWriter : IResponseBodyWriter
{
    private DefaultHttpResponseBodyWriter() {}
    
    public async Task WriteAsync(Stream stream, CancellationToken cancellationToken)
    {
        StreamWriter writer = new(stream);

        await writer.WriteAsync(
            @"
        <html>
            <head>
                <title>This is the title of the webpage!</title>
            </head>
            <body>
                <p>Welcome to Neko Web Server.</p>
            </body>
        </html>"
        );

        await writer.FlushAsync(cancellationToken);
    }

    public static readonly DefaultHttpResponseBodyWriter Instance = new();
}