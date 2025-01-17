using System.Net;
using System.Net.Sockets;
using WebServer.Domain.Core.Client;
using WebServer.Domain.Core.Request;
using WebServer.Domain.Core.Response;
using WebServer.Domain.Interfaces.Server;
using WebServer.Persistence.Server;
using WebServer.Tasks;

namespace WebServer;

public class Worker(
    ILogger<Worker> logger,
    WebServerConfiguration config,
    IRequestReader requestReader) : BackgroundService
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
        var token = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, cancellationToken).Token;

        // read request from socket
        HttpRequest request = await requestReader.ReadRequestAsync(clientSocket, token);

        // handle the request
        
        // create response
        HttpResponse response = new()
        {
            ContentLength = 350,
            ResponseBodyWriter = DefaultHttpResponseBodyWriter.Instance
        };
        
        // send response
        await SendResponseAsync(clientSocket, response, token);

        clientSocket.Close();
    }

    private async Task SendResponseAsync(Socket socket, HttpResponse response, CancellationToken stoppingToken)
    {
        NetworkStream stream = new(socket);
        StreamWriter streamWriter = new(stream);

        await streamWriter.WriteLineAsync($"{response.Version} {response.StatusCode} {response.StatusText}");
        await streamWriter.WriteLineAsync($"Content-Length: {response.ContentLength}");
        await streamWriter.WriteLineAsync($"Content-Type: {response.ContentType}");
        await streamWriter.WriteLineAsync();
        await streamWriter.FlushAsync(stoppingToken);
        if (response.ContentLength > 0)
        {
            await response.ResponseBodyWriter.WriteAsync(stream, stoppingToken);
        }
        await stream.FlushAsync(stoppingToken);
    }
}