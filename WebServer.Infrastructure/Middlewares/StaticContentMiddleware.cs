using System.Net;
using WebServer.Domain.Core.Request;
using WebServer.Domain.Core.Response;
using WebServer.Domain.Interfaces.Middlewares;
using WebServer.Infrastructure.Server;

namespace WebServer.Infrastructure.Middlewares;

public class StaticContentMiddleware : Middleware
{
    public override async Task<HttpResponse> InvokeNextAsync(HttpRequest request)
    {
        if (request.RequestLine.Method == HttpMethod.Get && request.RequestLine is { Version: "HTTP/1.1" })
        {
            return await GetResult(request);
        }

        return await base.InvokeNextAsync(request);
    }

    protected override async Task<HttpResponse> GetResult(HttpRequest? request)
    {
        var root = "D:\\Self-Learning\\.Net\\BrokenWebServer";

        var url = request!.RequestLine.Uri;
        if (url.StartsWith("/"))
        {
            url = url[1..];
        }

        url = url.Replace("/", "\\");

        HttpResponse response = new();

        var file = new FileInfo(Path.Combine(root, url));
        if (file.Exists)
        {

            var fileContent = await File.ReadAllTextAsync(file.FullName);
            response.ContentLength = fileContent.Length;
            response.ContentType = "text/html";
            response.ResponseBodyWriter = new HttpResponseBodyWriter(fileContent);
            response.StatusCode = HttpStatusCode.Accepted;
            response.StatusText = "ACCEPTED";
        }
        else
        {
            response = new HttpResponse
            {
                ContentLength = 350,
                ResponseBodyWriter = DefaultHttpResponseBodyWriter.Instance
            };
        }

        return await Task.FromResult(response);
    }
}