using Microsoft.Extensions.Primitives;
using WebServer.Domain.Abstractions.Builders;
using WebServer.Domain.Core.Request;

namespace WebServer.Infrastructure.Builder;

public class HttpRequestBuilder : IHttpRequestBuilder
{
    private HttpRequestLine HttpRequestLine { get; set; } = new();
    private string Host { get; set; } = string.Empty;
    private bool IsKeepAlive { get; set; }
    private IDictionary<string, StringValues> Headers { get; set; } = new Dictionary<string, StringValues>();

    public HttpRequest Build()
    {
        Validate(); // It will throw if it fails

        return new HttpRequest
        {
            RequestLine = HttpRequestLine,
            Host = Host,
            IsKeepAlive = IsKeepAlive,
            Headers = Headers
        };
    }

    public IHttpRequestBuilder AddRequestLine(HttpRequestLine requestLine)
    {
        HttpRequestLine = requestLine;
        return this;
    }


    public IHttpRequestBuilder AddHost(string host)
    {
        Host = host;
        return this;
    }

    public IHttpRequestBuilder KeepAlive(bool isKeepAlive)
    {
        IsKeepAlive = isKeepAlive;
        return this;
    }

    public IHttpRequestBuilder AddHeaders(HttpHeader header)
    {
        if ("Host".Equals(header.Name, StringComparison.OrdinalIgnoreCase))
        {
            return AddHost(header.Values.First() ?? string.Empty);
        }

        if ("Connection".Equals(header.Name, StringComparison.OrdinalIgnoreCase))
        {
            return KeepAlive("keep-alive".Equals(header.Values.First(), StringComparison.OrdinalIgnoreCase));
        }

        if (!Headers.TryGetValue(header.Name, out var value))
        {
            Headers.Add(header.Name, header.Values);
        }
        else
        {
            Headers[header.Name] = StringValues.Concat(value, header.Values);
        }

        return this;
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Host))
        {
            throw new ArgumentNullException(nameof(Host));
        }
    }
}