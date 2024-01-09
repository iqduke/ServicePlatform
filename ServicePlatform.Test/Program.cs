using ServicePlatform.Test;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(p =>
{
    p.ServiceName = "iqduke service platform";
});
builder.Services.AddSystemd();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();