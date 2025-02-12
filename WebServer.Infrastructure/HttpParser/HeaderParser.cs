using WebServer.Domain.Abstractions.HttpParser;
using WebServer.Domain.Core.Request;

namespace WebServer.Infrastructure.HttpParser;

public class HeaderParser : IHttpParser<HttpHeader>
{
    public static HttpHeader TryParse(string line)
    {
        int idx = line.IndexOf(':');

        if (idx < 0) throw new MalformedRequestException();

        return new HttpHeader
        {
            Name = line[..idx].Trim(),
            Values = line[(idx + 1)..].Trim()
        };
    }
}