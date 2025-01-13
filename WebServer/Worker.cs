using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer.Domain.Core.Client;
using WebServer.Domain.Core.Request;
using WebServer.Infrastructure;
using WebServer.Infrastructure.Builder;
using WebServer.Infrastructure.HttpParser;
using WebServer.Persistence.Server;

namespace WebServer;

public class Worker(ILogger<Worker> logger, WebServerConfiguration config) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IPEndPoint endPoint = new(string.IsNullOrEmpty(config.Address)
                ? IPAddress.Any
                : IPAddress.Parse(config.Address),
            config.Port);

        Socket serverSocket = new(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        serverSocket.Bind(endPoint);

        logger.LogInformation("Listening... (port: {})", config.Port);
        serverSocket.Listen();

        List<ClientConnection> clientConnections = [];

        while (!stoppingToken.IsCancellationRequested)
        {
            var clientSocket = await serverSocket.AcceptAsync(stoppingToken)
                               ?? throw new OperationCanceledException();

            var connection = HandleNewClientConnectionAsync(clientSocket, stoppingToken);
            clientConnections.Add(new ClientConnection
            {
                Handler = connection
            });
        }

        Task.WaitAll(clientConnections.Select(c => c.Handler).ToArray(), stoppingToken);

        serverSocket.Close();
    }

    private async Task HandleNewClientConnectionAsync(Socket clientSocket, CancellationToken stoppingToken)
    {
        // Stop reading if exceeds 3 seconds
        var cancellationToken = new CancellationTokenSource(3000).Token;

        // read request from socket
        HttpRequest request = await ReadRequestAsync(clientSocket,
            CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, cancellationToken).Token);

        // handle the request
        // create response
        // send response

        clientSocket.Close();
    }

    private async Task<HttpRequest> ReadRequestAsync(Socket clientSocket, CancellationToken cancellationToken)
    {
        NetworkStream stream = new(clientSocket);
        StreamReader reader = new(stream, Encoding.ASCII);

        var requestLineRaw = await reader.ReadLineAsync(cancellationToken);
        logger.LogInformation(requestLineRaw);
        if (requestLineRaw != null)
        {
            try
            {
                HttpRequestLine requestLine = RequestLineParser.TryParse(requestLineRaw);
            }
            catch (MalformedRequestException ex)
            {
                logger.LogError(ex, ex.Message);
            }

            var headerLine = await reader.ReadLineAsync(cancellationToken);
            while (headerLine != null)
            {
                logger.LogInformation(headerLine);
                headerLine = await reader.ReadLineAsync(cancellationToken);
            }

        }

        return new HttpRequestBuilder().Build();
    }
}