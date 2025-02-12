using System.Net.Sockets;
using WebServer.Domain.Core.Response;
using WebServer.Domain.Interfaces.Server;

namespace WebServer.Tasks;

public class HttpResponseWriter(Socket socket, HttpResponse response) : IResponseWriter
{
    public async Task WriteAsync(CancellationToken stoppingToken)
    {
        NetworkStream stream = new(socket);
        StreamWriter streamWriter = new(stream);

        await streamWriter.WriteLineAsync($"{response.Version} {(int)response.StatusCode} {response.StatusText}");
        await streamWriter.WriteLineAsync($"Content-Length: {response.ContentLength}");
        await streamWriter.WriteLineAsync($"Content-Type: {response.ContentType}");
        await streamWriter.WriteLineAsync($"Connection: close");
        await streamWriter.WriteLineAsync();
        await streamWriter.FlushAsync(stoppingToken);
        if (response.ContentLength > 0)
        {
            await response.ResponseBodyWriter.WriteAsync(stream, stoppingToken);
        }

        await stream.FlushAsync(stoppingToken);
    }
}