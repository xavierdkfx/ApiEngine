using Microsoft.Extensions.Logging;

namespace ApiEngine;

public class StartupWebComponent : IWebComponent
{
    public void Load(WebApplicationBuilder builder, ComponentContext componentContext)
    {
        builder.Logging.AddConsoleFormatter();
        builder.Host.UseNLog(); 
    }
}