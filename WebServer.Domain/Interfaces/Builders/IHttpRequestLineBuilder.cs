using WebServer.Domain.Core.Request;

namespace WebServer.Domain.Abstractions.Builders;

public interface IHttpRequestLineBuilder : IHttpBuilder<HttpRequestLine>
{
    IHttpRequestLineBuilder AddMethod(HttpMethod method);
    IHttpRequestLineBuilder AddUri(string uri);
    IHttpRequestLineBuilder AddVersion(string version);
}