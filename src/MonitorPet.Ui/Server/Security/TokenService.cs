using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MonitorPet.Ui.Server.Security;

public static class TokenService
{
    public static string GenerateToken(string key, Claim[] claims, int maxMinute = 120)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.ASCII.GetBytes(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(maxMinute),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static Claim[] TryGetClaimsFromToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return new Claim[0];

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var claimsPrincipal =
                tokenHandler.ReadJwtToken(token);

            return claimsPrincipal.Claims.ToArray();
        }
        catch
        {
            return new Claim[0];
        }
    }
}