﻿namespace ApiEngine.Handlers;

public class ExceptionHandler : IGlobalExceptionHandler, ISingleton
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        return Task.CompletedTask;
    }
}