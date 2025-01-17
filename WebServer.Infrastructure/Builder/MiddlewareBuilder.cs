using WebServer.Domain.Interfaces.Builders;
using WebServer.Domain.Interfaces.Middlewares;

namespace WebServer.Infrastructure.Builder;

public class MiddlewareBuilder : IMiddlewareBuilder
{
    private Middleware? Middleware { get; set; }

    public Middleware? Build()
    {
        return Middleware;
    }

    public MiddlewareBuilder SetNext(Middleware middleware)
    {
        if (Middleware != null) middleware.Next = Middleware;
        Middleware = middleware;

        return this;
    }
}