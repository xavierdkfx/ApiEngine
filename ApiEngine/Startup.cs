namespace ApiEngine;

public static class Startup
{
    public static RunOptions EngineStartup(this RunOptions runOptions)
    {
        return runOptions
            .AddWebComponent<StartupWebComponent>()
            .AddComponent<StartupServiceComponent>()
            .UseComponent<StartupApplicationComponent>();
    }
}