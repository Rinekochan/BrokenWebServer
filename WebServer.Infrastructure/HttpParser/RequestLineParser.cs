using WebServer.Domain.Abstractions.HttpParser;
using WebServer.Domain.Core.Request;
using WebServer.Infrastructure.Builder;

namespace WebServer.Infrastructure.HttpParser;

public class RequestLineParser : IHttpParser<HttpRequestLine>
{
    public static HttpRequestLine TryParse(string line)
    {
        string[] parts = line.Split(" ");

        if (parts.Length != 3) throw new MalformedRequestException();

        if (!IsMethodValid(parts[0])) throw new MalformedRequestException("The request method is not in HTTP format");
        if (!IsUriValid(parts[1])) throw new MalformedRequestException("The request uri path is not valid");
        if (!IsVersionValid(parts[2])) throw new MalformedRequestException("The request version is not valid");

        return new HttpRequestLineBuilder()
            .AddMethod(new HttpMethod(parts[0]))
            .AddUri(parts[1])
            .AddVersion(parts[2])
            .Build();
    }

    private static bool IsMethodValid(string method)
    {
        return method.Equals("GET", StringComparison.OrdinalIgnoreCase)
               || method.Equals("HEAD", StringComparison.OrdinalIgnoreCase)
               || method.Equals("CONNECT", StringComparison.OrdinalIgnoreCase)
               || method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase)
               || method.Equals("POST", StringComparison.OrdinalIgnoreCase)
               || method.Equals("PUT", StringComparison.OrdinalIgnoreCase)
               || method.Equals("DELETE", StringComparison.OrdinalIgnoreCase)
               || method.Equals("TRACE", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsUriValid(string uri)
    {
        return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
    }

    private static bool IsVersionValid(string version)
    {
        return version.Equals("HTTP/1.0", StringComparison.InvariantCultureIgnoreCase)
               || version.Equals("HTTP/1.1", StringComparison.InvariantCultureIgnoreCase)
               || version.Equals("HTTP/2", StringComparison.InvariantCultureIgnoreCase)
               || version.Equals("HTTP/3", StringComparison.InvariantCultureIgnoreCase);
    }
}