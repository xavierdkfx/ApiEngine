namespace ApiEngine.Aop.Models;

public class HttpTemp
{
    #region 属性

    public ActionExecutingContext ExecutingContext { get; set; }
    public ActionExecutedContext ExecutedContext { get; set; }
    public AppInfoOptions.LogClass LogOptions => App.GetOptionsMonitor<AppInfoOptions>().Log;

    public string Method => ExecutingContext.HttpContext.Request.Method;
    public string Path => ExecutingContext.HttpContext.Request.Path.Value;
    public string Arguments => ExecutingContext.ActionArguments.ToJson();

    public bool NoLogInputFlag => !LogOptions.Request
                                  || LogOptions.IgnoreMethods.ContainsIgnoreCase(Method)
                                  || LogOptions.IgnorePaths.ContainsIgnoreCase(Path);

    public bool NoLogOutputFlag => !LogOptions.Response
                                   || LogOptions.IgnoreMethods.ContainsIgnoreCase(Method)
                                   || LogOptions.IgnorePaths.ContainsIgnoreCase(Path);

    #endregion

    #region 方法

    public void LogInput<T>() where T : class
    {
        if (NoLogInputFlag)
        {
            return;
        }

        $"{Method} {Path} {Arguments}".LogInformation<T>();
    }

    public void LogOutput<T>() where T : class
    {
        if (NoLogOutputFlag || ExecutedContext.Result == null)
        {
            return;
        }

        if (ExecutedContext.Exception != null)
        {
            $"{ExecutedContext.Result.ToJson()}".LogError<T>();
        }
        else
        {
            $"{ExecutedContext.Result.ToJson()}".LogInformation<T>();
        }
    }

    public void VerifyModelState()
    {
        if (ExecutedContext.ModelState.IsValid)
        {
            return;
        }

        var result = (RESTfulResult<object>)((JsonResult)ExecutedContext.Result)?.Value;
        if (result != null)
        {
            result.Errors = ExecutedContext.ModelState.Values.SelectMany(m => m.Errors).Select(m => m.ErrorMessage)
                .ToArray().Reverse().StringJoin(" ");
        }
    }

    #endregion
}