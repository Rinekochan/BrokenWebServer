using WebServer.Domain.Core.Request;

namespace WebServer.Domain.Abstractions.Builders;

public interface IHttpRequestBuilder : IHttpBuilder<HttpRequest>
{
    IHttpRequestBuilder AddRequestLine(HttpRequestLine requestLine);
    IHttpRequestBuilder AddHost(string host);
    IHttpRequestBuilder KeepAlive(bool isKeepAlive);
    IHttpRequestBuilder AddHeaders(HttpHeader header);
}