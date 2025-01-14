using Microsoft.Extensions.Primitives;

namespace WebServer.Domain.Core.Request;

public record HttpHeader
{
    public required string Name { get; set; }
    public required StringValues Values { get; set; }
}