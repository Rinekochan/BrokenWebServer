using WebServer.Domain.Core.Request;
using WebServer.Domain.Core.Response;
using WebServer.Domain.Interfaces.Middlewares;
using WebServer.Infrastructure.Server;

namespace WebServer.Infrastructure.Middlewares;

public class StaticContentMiddleware : Middleware
{
    public override async Task<HttpResponse> InvokeNextAsync(HttpRequest request)
    {
        if (request.RequestLine.Method == HttpMethod.Get && request.RequestLine is { Version: "HTTP/1.1", Uri: "/" or "/index.html" })
        {
            return await GetResult(request);
        }

        return await base.InvokeNextAsync(request);
    }

    protected override Task<HttpResponse> GetResult(HttpRequest? request)
    {
        return Task.FromResult(
            new HttpResponse
            {
                ContentLength = 350,
                ResponseBodyWriter = DefaultHttpResponseBodyWriter.Instance
            }
        );
    }
}