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
                <p>This is an example paragraph. Anything in the <strong>body</strong> tag will appear on the page, just like this <strong>p</strong> tag and its contents.</p>
            </body>
        </html>"
        );

        await writer.FlushAsync(cancellationToken);
    }

    public static readonly DefaultHttpResponseBodyWriter Instance = new();
}