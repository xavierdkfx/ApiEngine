namespace ApiEngine.Aop;

public class AuditFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpTemp = new HttpTemp { ExecutingContext = context };
        httpTemp.LogInput<AuditFilter>();

        httpTemp.ExecutedContext = await next();
        httpTemp.VerifyModelState();
        httpTemp.LogOutput<AuditFilter>();
    }
}