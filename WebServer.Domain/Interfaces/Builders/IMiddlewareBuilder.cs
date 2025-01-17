using WebServer.Domain.Abstractions.Builders;
using WebServer.Domain.Interfaces.Middlewares;

namespace WebServer.Domain.Interfaces.Builders;

public interface IMiddlewareBuilder : IHttpBuilder<Middleware?>
{
}