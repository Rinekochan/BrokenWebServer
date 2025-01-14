using WebServer;
using WebServer.Persistence.Server;

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

var host = builder.Build();
host.Run();