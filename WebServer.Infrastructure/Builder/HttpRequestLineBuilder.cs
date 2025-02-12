using WebServer.Domain.Abstractions.Builders;
using WebServer.Domain.Core.Request;

namespace WebServer.Infrastructure.Builder;

public class HttpRequestLineBuilder : IHttpRequestLineBuilder
{
    private HttpMethod? Method { get; set; }
    private string Uri { get; set; } = string.Empty;
    private string Version { get; set; } = string.Empty;

    public HttpRequestLine Build()
    {
        Validate(); // It will throw if it fails

        return new HttpRequestLine
        {
            Method = Method!,
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

    private void Validate()
    {
        if (Method == null)
        {
            throw new ArgumentNullException(nameof(Method));
        }

        if (string.IsNullOrEmpty(Uri))
        {
            throw new ArgumentNullException(nameof(Uri));
        }

        if (string.IsNullOrEmpty(Version))
        {
            throw new ArgumentNullException(nameof(Version));
        }
    }
}