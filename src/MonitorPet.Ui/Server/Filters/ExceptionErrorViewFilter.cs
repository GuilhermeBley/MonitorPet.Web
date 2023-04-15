using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MonitorPet.Ui.Server.Filters;

public class ExceptionErrorViewFilter : ExceptionFilterAttribute
{
    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        var coreException = context.Exception as Core.Exceptions.ICoreException;
        if (coreException is null)
            return;

        var result = new ObjectResult(new Shared.Model.Exceptions.ErrorViewModel[]
        {
            new Shared.Model.Exceptions.ErrorViewModel(coreException.StatusCode, coreException.Message)
        })
        {
            StatusCode = coreException.StatusCode
        };
        
        context.Result = result;
        await Task.CompletedTask;
    }
}