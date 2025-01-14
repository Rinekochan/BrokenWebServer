using WebServer.Domain.Core.Request;

namespace WebServer.Domain.Abstractions.Builders;

public interface IHttpRequestBuilder : IHttpBuilder<HttpRequest>
{
    IHttpRequestBuilder AddMethod(HttpMethod method);
    IHttpRequestBuilder AddUri(string uri);
    IHttpRequestBuilder AddVersion(string version);
    IHttpRequestBuilder AddRequestLine(HttpRequestLine requestLine);
}