namespace WebServer.Domain.Core.Client;

public record ClientConnection
{
    public required Task Handler { get; set; }
}