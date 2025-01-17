using WebServer;
using WebServer.Domain.Interfaces.Factories;
using WebServer.Domain.Interfaces.Server;
using WebServer.Factories;
using WebServer.Persistence.Server;
using WebServer.Tasks;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddLogging(logging =>
    logging.AddSimpleConsole(options =>
    {
        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    })
);

builder.Services.AddSingleton
    (builder.Configuration.GetRequiredSection("Server").Get<WebServerConfiguration>()
    ?? new WebServerConfiguration());

builder.Services.AddTransient<IHttpRequestReaderFactory, HttpRequestReaderFactory>();
builder.Services.AddTransient<IHttpResponseWriterFactory, HttpResponseWriterFactory>();

var host = builder.Build();
host.Run();