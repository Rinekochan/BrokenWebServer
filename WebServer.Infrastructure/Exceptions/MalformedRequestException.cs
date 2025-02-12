namespace WebServer.Infrastructure;

public class MalformedRequestException : Exception
{
    public MalformedRequestException() : base("The received request is invalid.") { }
    public MalformedRequestException(string message) : base(message) { }

}