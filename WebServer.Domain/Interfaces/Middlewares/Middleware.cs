using WebServer.Domain.Core.Request;
using WebServer.Domain.Core.Response;

namespace WebServer.Domain.Interfaces.Middlewares;

public abstract class Middleware()
{
    public Middleware? Next { get; set; }

    public virtual async Task<HttpResponse> InvokeNextAsync(HttpRequest request)
    {
        return Next == null ? await GetResult(null) : await Next.InvokeNextAsync(request);
    }

    protected abstract Task<HttpResponse> GetResult(HttpRequest? request);
}