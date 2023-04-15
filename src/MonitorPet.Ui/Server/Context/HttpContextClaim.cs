using MonitorPet.Application.Security;

namespace MonitorPet.Ui.Server.Context;

public class HttpContextClaim : ContextClaim
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private HttpContext _current => _httpContextAccessor.HttpContext
        ?? throw new ArgumentNullException("Falha em coleta de contexto.");

    public HttpContextClaim(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<CurrentClaim?> GetCurrentClaim()
    {
        await Task.CompletedTask;
        return new CurrentClaim(
            _current.User.Claims);
    }
}

