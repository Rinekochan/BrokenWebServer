using WebServer.Domain.Abstractions.Builders;
using WebServer.Domain.Core.Request;

namespace WebServer.Infrastructure.Builder;

public class HttpRequestLineBuilder : IHttpRequestLineBuilder
{
    private HttpMethod Method { get; set; } = HttpMethod.Get;
    private string Uri { get; set; } = string.Empty;
    private string Version { get; set; } = string.Empty;

    public HttpRequestLine Build()
    {
        return new HttpRequestLine
        {
            Method = Method,
            Uri = Uri,
            Version = Version
        };
    }

    public IHttpRequestLineBuilder AddMethod(HttpMethod method)
    {
        Method = method;
        return this;
    }

    public IHttpRequestLineBuilder AddUri(string uri)
    {
        Uri = uri;
        return this;
    }

    public IHttpRequestLineBuilder AddVersion(string version)
    {
        Version = version;
        return this;
    }
}