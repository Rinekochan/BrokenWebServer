using System.Net;
using WebServer.Domain.Core.Server;
using WebServer.Domain.Interfaces.Server;

namespace WebServer.Domain.Core.Response;

public record HttpResponse
{
    public string Version { get; set; } = "HTTP/1.1";
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;
    public string StatusText { get; set; } = nameof(StatusCode);
    public int ContentLength { get; set; } = 0;
    public string ContentType { get; set; } = "text/html";
    public IResponseBodyWriter ResponseBodyWriter { get; set; } = NullResponseBodyWriter.Instance;
}