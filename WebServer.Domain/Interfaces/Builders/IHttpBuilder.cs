namespace WebServer.Domain.Abstractions.Builders;

public interface IHttpBuilder<out T>
{
    T Build();
}