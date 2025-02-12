namespace WebServer.Domain.Core.Request;

public record HttpRequestLine
{
    public HttpMethod Method { get; set; }
    public string Uri { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}