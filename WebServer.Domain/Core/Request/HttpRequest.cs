using Microsoft.Extensions.Primitives;

namespace WebServer.Domain.Core.Request;

public record HttpRequest
{
    public HttpRequestLine RequestLine { get; set; }
    public required string Host { get; set; }
    public bool IsKeepAlive { get; set; } = false;
    public required IDictionary<string, StringValues> Headers { get; set; }
}