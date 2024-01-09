namespace ServicePlatform;

public abstract class TimerScheduledService : BackgroundService
{
    private readonly PeriodicTimer _timer;
    private readonly DateTime _startTime;
    private readonly ILogger _logger;

    /// <summary>
    /// 定时服务
    /// </summary>
    /// <param name="logger">日志</param>
    /// <param name="period">时间周期</param>
    /// <param name="startTime">开始执行时间</param>
    protected TimerScheduledService(ILogger logger,TimeSpan period,  DateTime startTime)
    {
        _logger = logger;
        _startTime = startTime;
        _timer = new PeriodicTimer(period);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service is start.");
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delay = (_startTime - DateTime.Now);
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(_startTime -DateTime.Now , stoppingToken);
        }
        
        try
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    _logger.LogInformation("Begin execute service");
                    await ExecuteInternal(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Execute exception");
                }
                finally
                {
                    _logger.LogInformation("Execute finished");
                }
            }
        }
        catch (OperationCanceledException operationCancelledException)
        {
            _logger.LogWarning(operationCancelledException, "service stopped");
        }
    }

    protected abstract Task ExecuteInternal(CancellationToken stoppingToken);

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service is stopping.");
        _timer.Dispose();
        return base.StopAsync(cancellationToken);
    }
}