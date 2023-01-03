namespace ApiEngine;

internal sealed class StartupServiceComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        // 跨域
        services.AddCorsAccessor();
        // 限流
        Settings.SetRateLimit(services);
        // 健康检查
        services.AddHealthChecks();
        // 配置
        services.AddConfigurableOptions<AppInfoOptions>();
        // JWT授权
        services.AddJwt<JwtHandler>(enableGlobalAuthorize: App.GetOptionsMonitor<AppInfoOptions>().GlobalAuthorize);
        // 审计
        services.AddMvcFilter<AuditFilter>();
        // 控制器.设置JSON.规范化结果
        services.AddControllers().AddNewtonsoftJson(Settings.SetJsonOptions).AddInjectWithUnifyResult();
        // 设置数据库
        Settings.SetSqlSugar();
        // 远程请求
        services.AddRemoteRequest();
        // 任务调度
        services.AddSchedule(Settings.SetScheduleOptions);
        //日志
        Settings.SetLog(services);
    }
}