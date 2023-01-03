namespace ApiEngine.Background;

public class LogJob : IJob
{
    private readonly AppInfoOptions _options;

    public LogJob(IOptionsMonitor<AppInfoOptions> options)
    {
        _options = options.CurrentValue;
    }

    /// <summary>
    ///     清理数据库日志（仅数据库模式有用）
    /// </summary>
    /// <param name="context"></param>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        if (_options.Log.LogDbSet != null)
        {
            var db = DbScoped.SugarScope.AsTenant().GetConnection("log");
            var keepDate = db.GetDate().AddMonths(_options.Log.LogDbSet.KeepMonths * -1);
            await db.Deleteable<LogMod>().Where(w => w.LongDate < keepDate).ExecuteCommandAsync();
        }
    }
}