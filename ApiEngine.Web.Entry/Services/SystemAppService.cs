namespace ApiEngine.Web.Entry.Services;

/// <summary>
///     系统服务接口
/// </summary>
[AllowAnonymous]
public class SystemAppService : IDynamicApiController, ITransient
{
    /// <summary>
    ///     服务器日期时间
    /// </summary>
    /// <returns></returns>
    public DateTime GetDate()
    {
        return DbScoped.SugarScope.GetDate();
    }

    public JsonDto Test(JsonDto dto)
    {
        "Warning".LogWarning();
        return dto;
    }
}