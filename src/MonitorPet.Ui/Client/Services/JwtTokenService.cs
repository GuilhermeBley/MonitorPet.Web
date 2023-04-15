using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MonitorPet.Ui.Client.Services;

public class JwtTokenService
{
    public TokenInfo GetTokenInfo(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return TokenInfo.Unauthorized;

        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token))
            return TokenInfo.Unauthorized;

        var tokenJwtRead = tokenHandler.ReadJwtToken(token);

        return new TokenInfo(tokenJwtRead.Claims.ToArray(), tokenJwtRead.ValidTo);
    }

    public class TokenInfo
    {
        public static TokenInfo Unauthorized = new() { Authorized = false };
        public bool Authorized { get; private set; } = true;
        public Claim[] Claims { get; } = new Claim[0];
        public DateTime? Expiration { get; }
        public KeyValuePair<string, string>[] TupleClaim { get; } = new KeyValuePair<string, string>[0];

        private TokenInfo()
        {

        }

        public TokenInfo(Claim[] claims, DateTime expirations)
        {
            Claims = claims;
            TupleClaim = claims.Select(c => KeyValuePair.Create(c.Type, c.Value)).ToArray();
            Expiration = expirations;
        }

        public bool IsExpired()
        {
            if (Expiration is null)
                return true;

            return Expiration.Value < DateTime.UtcNow;
        }
    }
}

