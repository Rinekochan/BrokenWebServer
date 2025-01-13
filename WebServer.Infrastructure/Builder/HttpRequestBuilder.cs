using WebServer.Domain.Abstractions.Builders;
using WebServer.Domain.Core.Request;

namespace WebServer.Infrastructure.Builder;

public class HttpRequestBuilder : IHttpBuilder<HttpRequest>
{
    private HttpRequestLine HttpRequestLine { get; set; } = new();

    public HttpRequest Build()
    {
        return new HttpRequest
        {
            RequestLine = HttpRequestLine
        };
    }

    public HttpRequestBuilder AddMethod(HttpMethod method)
    {
        HttpRequestLine.Method = method;
        return this;
    }

    public HttpRequestBuilder AddUri(string uri)
    {
        HttpRequestLine.Uri = uri;
        return this;
    }

    public HttpRequestBuilder AddVersion(string version)
    {
        HttpRequestLine.Version = version;
        return this;
    }

    public HttpRequestBuilder AddRequestLine(HttpRequestLine requestLine)
    {
        HttpRequestLine = requestLine;
        return this;
    }
}