using System.Net.Sockets;
using System.Text;
using WebServer.Domain.Core.Request;
using WebServer.Domain.Interfaces.Server;
using WebServer.Infrastructure;
using WebServer.Infrastructure.Builder;
using WebServer.Infrastructure.HttpParser;

namespace WebServer.Tasks;

public class HttpRequestReader(ILogger<HttpRequestReader> logger) : IRequestReader
{
    public async Task<HttpRequest> ReadRequestAsync(Socket socket, CancellationToken cancellationToken)
    {
        NetworkStream stream = new(socket);
        StreamReader reader = new(stream, Encoding.ASCII);

        var requestLineRaw = await reader.ReadLineAsync(cancellationToken);
        logger.LogInformation(requestLineRaw);

        var request = new HttpRequestBuilder();

        if (requestLineRaw != null)
        {
            try
            {
                HttpRequestLine requestLine = RequestLineParser.TryParse(requestLineRaw);
                request.AddRequestLine(requestLine);
            }
            catch (MalformedRequestException ex)
            {
                logger.LogError(ex, ex.Message);
            }


            var headerLineRaw = await reader.ReadLineAsync(cancellationToken);
            while (!string.IsNullOrEmpty(headerLineRaw))
            {
                HttpHeader headerLine = HeaderParser.TryParse(headerLineRaw);

                if (!string.IsNullOrEmpty(headerLineRaw))
                {
                    request.AddHeaders(headerLine);
                }

                logger.LogInformation(headerLineRaw);
                headerLineRaw = await reader.ReadLineAsync(cancellationToken);
            }
        }

        return request.Build();
    }
}