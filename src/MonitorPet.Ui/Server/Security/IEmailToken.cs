using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonitorPet.Ui.Server.Security
{
    public interface IEmailToken
    {
        string GenerateToken(Claim[] claims);
    }
    public class EmailToken : IEmailToken
    {
        public const int MAX_MINUTE = 10;
        private readonly Options.JwtOptions _options;
        public EmailToken(IOptions<Options.JwtOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateToken(Claim[] claims)
        {
            return TokenService.GenerateToken(_options.KeyEmail, claims, MAX_MINUTE);
        }
    }
}
