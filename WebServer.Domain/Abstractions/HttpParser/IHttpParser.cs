namespace WebServer.Domain.Abstractions.HttpParser;

public interface IHttpParser<out T>
{
    static abstract T TryParse(string line);
}