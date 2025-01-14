using Microsoft.Extensions.Primitives;
using WebServer.Domain.Abstractions.Builders;
using WebServer.Domain.Core.Request;

namespace WebServer.Infrastructure.Builder;

public class HttpRequestBuilder : IHttpRequestBuilder
{
    private HttpRequestLine HttpRequestLine { get; set; } = new();
    private string Host { get; set; } = string.Empty;
    private bool IsKeepAlive { get; set; } = false;
    private IDictionary<string, StringValues> Headers { get; set; } = new Dictionary<string, StringValues>();
    
    public HttpRequest Build()
    {
        return new HttpRequest
        {
            RequestLine = HttpRequestLine,
            Host = Host,
            IsKeepAlive = IsKeepAlive,
            Headers = Headers
        };
    }

    public IHttpRequestBuilder AddMethod(HttpMethod method)
    {
        HttpRequestLine.Method = method;
        return this;
    }

    public IHttpRequestBuilder AddUri(string uri)
    {
        HttpRequestLine.Uri = uri;
        return this;
    }

    public IHttpRequestBuilder AddVersion(string version)
    {
        HttpRequestLine.Version = version;
        return this;
    }

    public IHttpRequestBuilder AddRequestLine(HttpRequestLine requestLine)
    {
        HttpRequestLine = requestLine;
        return this;
    }
}