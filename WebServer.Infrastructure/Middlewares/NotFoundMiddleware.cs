using System.Net;
using WebServer.Domain.Core.Request;
using WebServer.Domain.Core.Response;
using WebServer.Domain.Interfaces.Middlewares;

namespace WebServer.Infrastructure.Middlewares;

public class NotFoundMiddleware : Middleware
{
    protected override Task<HttpResponse> GetResult(HttpRequest? request)
    {
        return Task.FromResult(
            new HttpResponse
            {
                ContentLength = 0,
                StatusCode = HttpStatusCode.NotFound,
                StatusText = nameof(HttpStatusCode.NotFound),
                ContentType = "text/html"
            }
        );
    }
}