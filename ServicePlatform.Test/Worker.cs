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