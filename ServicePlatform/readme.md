## Work Service 定时服务

```
NuGet\Install-Package Microsoft.Extensions.Hosting.WindowsServices -Version 8.0.0
NuGet\Install-Package Microsoft.Extensions.Hosting.Systemd -Version 8.0.0
```

```
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
```


``` 
namespace ServicePlatform.Test;
public class Worker : TimerScheduledService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger) : base(logger, TimeSpan.FromSeconds(1), DateTime.Now.AddSeconds(5) )
    {
        _logger = logger;
    }

    protected override Task ExecuteInternal(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
}
```

```
https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service
```