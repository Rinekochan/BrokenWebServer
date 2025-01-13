using WebServer.Domain.Abstractions.HttpParser;
using WebServer.Domain.Core.Request;

namespace WebServer.Infrastructure.HttpParser;

public class RequestParser : IHttpParser<HttpRequest>
{
    public static HttpRequest TryParse(string line)
    {
        return new HttpRequest();
    }
}