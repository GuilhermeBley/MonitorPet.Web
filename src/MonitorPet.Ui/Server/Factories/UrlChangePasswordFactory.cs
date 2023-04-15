using MonitorPet.Application.Model.User;
using MonitorPet.Infrastructure.Factories;
using MonitorPet.Ui.Server.Security;
using System.Security.Claims;

namespace MonitorPet.Ui.Server.Factories;

public class UrlChangePasswordFactory : IUrlChangePasswordFactory
{
    private readonly IEmailToken _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private HttpContext _current => _httpContextAccessor.HttpContext
        ?? throw new ArgumentNullException("Falha em coleta de contexto.");
    public string AppBaseUrl => $"{_current.Request.Scheme}://{_current.Request.Host}";

    public UrlChangePasswordFactory(IEmailToken tokenService,
        IHttpContextAccessor httpContextAccessor)
    {
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Uri> Create(UserModel userToCreateUrl, IEnumerable<Claim> claims)
    {
        var token = _tokenService.GenerateToken(claims.ToArray());
        return await Task.FromResult(
            new Uri(AppBaseUrl+$"/ChangePassword/{token}/Change"));
    }
}

