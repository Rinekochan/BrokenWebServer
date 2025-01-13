using WebServer.Domain.Abstractions.Builders;
using WebServer.Domain.Core.Request;

namespace WebServer.Infrastructure.Builder;

public class HttpRequestLineBuilder : IHttpBuilder<HttpRequestLine>
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

    public HttpRequestLineBuilder AddMethod(HttpMethod method)
    {
        Method = method;
        return this;
    }

    public HttpRequestLineBuilder AddUri(string uri)
    {
        Uri = uri;
        return this;
    }

    public HttpRequestLineBuilder AddVersion(string version)
    {
        Version = version;
        return this;
    }
}