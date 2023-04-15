using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonitorPet.Ui.Server.Security
{
    public interface IApiToken
    {
        string GenerateToken(Claim[] claims);
    }
    public class ApiToken : IApiToken
    {
        private readonly Options.JwtOptions _options;
        public ApiToken(IOptions<Options.JwtOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateToken(Claim[] claims)
        {
            return TokenService.GenerateToken(_options.Key, claims);
        }
    }
}
