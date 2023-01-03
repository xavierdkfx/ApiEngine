namespace ApiEngine.Handlers;

public class JwtHandler : AppAuthorizeHandler
{
    /// <summary>
    ///     重写 Handler 添加自动刷新收取逻辑
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
        {
            await AuthorizeHandleAsync(context);
        }
        else
        {
            context.Fail();

            var currentHttpContext = context.GetCurrentHttpContext();
            if (currentHttpContext is null)
            {
                return;
            }

            currentHttpContext.SignoutToSwagger();
        }
    }

    /// <summary>
    ///     请求管道
    /// </summary>
    /// <param name="context"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public override Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
    {
        // 此处已经自动验证 Jwt 的有效性了，无需手动验证
        return Task.FromResult(CheckAuthorzie(httpContext));
    }

    /// <summary>
    ///     检查权限
    /// </summary>
    /// <param name="_"></param>
    /// <returns></returns>
    private static bool CheckAuthorzie(HttpContext _)
    {
        return true;
    }
}