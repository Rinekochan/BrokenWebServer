using System.Net;
using System.Net.Sockets;
using WebServer.Domain.Core.Client;
using WebServer.Domain.Core.Request;
using WebServer.Domain.Core.Response;
using WebServer.Domain.Interfaces.Factories;
using WebServer.Domain.Interfaces.Middlewares;
using WebServer.Infrastructure.Builder;
using WebServer.Infrastructure.Middlewares;
using WebServer.Persistence.Server;

namespace WebServer;

public class Worker(
    ILogger<Worker> logger,
    WebServerConfiguration config,
    IHttpRequestReaderFactory requestReaderFactory,
    IHttpResponseWriterFactory responseWriterFactory) : BackgroundService
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
        List<Task> activeConnections = [];

        while (!stoppingToken.IsCancellationRequested)
        {
            var clientSocket = await serverSocket.AcceptAsync(stoppingToken)
                               ?? throw new OperationCanceledException();
            
            activeConnections.Add(
                Task.Run(() =>
                {
                    var connection = HandleNewClientConnectionAsync(clientSocket, stoppingToken);
                    clientConnections.Add(new ClientConnection
                    {
                        Handler = connection
                    });
                }, stoppingToken));
            
            if (activeConnections.Any(connection => connection.IsCompleted))
            {
                activeConnections.RemoveAll(connection => connection.IsCompleted);
                logger.LogCritical(activeConnections.Count.ToString());
            }
            
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
        HttpRequest request = await requestReaderFactory.Create(clientSocket).ReadRequestAsync(token);

        // handle the request
        Middleware? middleware = new MiddlewareBuilder()
            .SetNext(new NotFoundMiddleware())
            .SetNext(new StaticContentMiddleware())
            .Build();
        
        // create response
        HttpResponse response;
        
        if (middleware != null) response = await middleware.InvokeNextAsync(request);
        else response = new HttpResponse();
        
        // send response
        await responseWriterFactory.Create(clientSocket, response).WriteAsync(token);

        clientSocket.Close();
    }

   
}