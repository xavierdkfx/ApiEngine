namespace ApiEngine;

internal sealed class Settings
{
    /// <summary>
    ///     设置Json序列化
    /// </summary>
    /// <param name="jsonOptions"></param>
    public static void SetJsonOptions(MvcNewtonsoftJsonOptions jsonOptions)
    {
        jsonOptions.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        jsonOptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        jsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    }

    /// <summary>
    ///     设置限流
    /// </summary>
    /// <param name="services"></param>
    public static void SetRateLimit(IServiceCollection services)
    {
        services.Configure<IpRateLimitOptions>(App.Configuration.GetSection("IpRateLimiting"));
        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }

    /// <summary>
    ///     设置数据库连接
    /// </summary>
    public static void SetSqlSugar()
    {
        SugarIocServices.AddSqlSugar(new List<IocConfig>(App.GetConfig<List<IocConfig>>("ConnectionConfigs")));

        //设置参数
        SugarIocServices.ConfigurationSugar(db =>
        {
            db.CurrentConnectionConfig.IsAutoCloseConnection = true;
            db.CurrentConnectionConfig.LanguageType = LanguageType.Chinese;
#if DEBUG
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                // 记录执行的SQL语句
                var sqlLog = pars.Aggregate(sql, (current, param) => current?.Replace(param.ParameterName, $"'{param.Value.ObjToString()}'"));
                sqlLog.LogInformation();
            };
            db.Aop.OnError = ex =>
            {
                // 记录错误
                ex.Message.LogError(ex);
            };
#endif
        });
    }

    /// <summary>
    ///     设置后台任务
    /// </summary>
    /// <param name="scheduleOptions"></param>
    public static void SetScheduleOptions(ScheduleOptionsBuilder scheduleOptions)
    {
#if DEBUG
        scheduleOptions.AddJob<LogJob>("logjob", Triggers.Minutely());
#else
        scheduleOptions.AddJob<LogJob>("logjob", Triggers.Monthly());
#endif
    }

    /// <summary>
    ///     设置日志
    /// </summary>
    /// <param name="services"></param>
    public static void SetLog(IServiceCollection services)
    {
        var appInfo = App.GetOptionsMonitor<AppInfoOptions>();
        switch (appInfo.Log.LogType)
        {
            case LogTypeEnum.Db:
                var dbLog = DbScoped.SugarScope.AsTenant().GetConnection("log");
                dbLog.DbMaintenance.CreateDatabase();
                dbLog.CodeFirst.As<LogMod>(appInfo.Log.LogDbSet.TableName).InitTables<LogMod>();

                LogManager.LoadConfiguration("nlog-db.config");
                LogManager.Configuration.Variables["ConnectionString"] = dbLog.CurrentConnectionConfig.ConnectionString;
                LogManager.Configuration.Variables["TableName"] = appInfo.Log.LogDbSet.TableName;

                services.AddLogDashboard(opt =>
                {
                    opt.UseDataBase(() => new SqlConnection(dbLog.CurrentConnectionConfig.ConnectionString), appInfo.Log.LogDbSet.TableName);
                    opt.CustomLogModel<RequestTraceLogModel>();
                });
                break;
            case LogTypeEnum.Seq:
                LogManager.LoadConfiguration("nlog-seq.config");
                services.AddLogDashboard(opt => opt.CustomLogModel<RequestTraceLogModel>());
                break;
            case LogTypeEnum.File:
            default:
                LogManager.LoadConfiguration("nlog-file.config");
                services.AddLogDashboard(opt => opt.CustomLogModel<RequestTraceLogModel>());
                break;
        }
    }
}