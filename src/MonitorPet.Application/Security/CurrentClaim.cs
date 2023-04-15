using MonitorPet.Core.Security;
using System.Security.Claims;

namespace MonitorPet.Application.Security;

public class CurrentClaim
{
    public IEnumerable<Claim> Claims { get; } = Enumerable.Empty<Claim>();
    public int IdUser { get; } = 0;
    public string? Email { get; } = null;
    public string? Name { get; } = null;
    public string? IdDosador { get; } = null;

    public int RequiredIdUser { get {
            if (IdUser == 0)
                throw new Core.Exceptions.UnauthorizedCoreException("Usuário não logado.");
            return IdUser;
        }}

    public string RequiredEmail { get {
            if (string.IsNullOrEmpty(Email))
                throw new Core.Exceptions.UnauthorizedCoreException("Usuário não logado.");
            return Email;
        }}

    public string RequiredIdDosador { get {
            if (string.IsNullOrEmpty(IdDosador))
                throw new Core.Exceptions.ForbiddenCoreException("Usuário não pode acessar dosador.");
            return IdDosador;
        }}

    public CurrentClaim(IEnumerable<Claim> claims)
    {
        Claims = claims;
        IdUser = TryGetUserId(claims);
        Email = TryGetFirstValue(claims, RoleClaim.DEFAULT_EMAIL_TYPE);
        IdDosador = TryGetFirstValue(claims, RoleClaim.DEFAULT_ID_DOSADOR);
        Name = TryGetFirstValue(claims, RoleClaim.DEFAULT_NAME);
    }

    public bool IsLogged()
    {
        if (IdUser == 0 ||
            Email is null)
            return false;

        return true;
    }

    public void ThrowIfIsntLogged()
    {
        if (!IsLogged())
            throw new Core.Exceptions.CommonCoreException();
    }

    /// <summary>
    /// Check if contains role
    /// </summary>
    /// <exception cref="Core.Exceptions.ForbiddenCoreException"></exception>
    public void ThrowIfIsntContainRole(string value)
    {
        if (!ContainsRole(value))
            throw new Core.Exceptions.ForbiddenCoreException($"Não contém Role '{value}'.");
    }

    public bool ContainsRole(string value)
        => ContainsClaimInEnum(Claims, new Claim(RoleClaim.DEFAULT_ROLE_TYPE, value));

    public string? TryGetFirstValue(string type)
        => TryGetFirstValue(Claims, type);

    private static bool ContainsClaimInEnum(IEnumerable<Claim> claims, Claim? claimToCheck)
        => claims.Any(claim => SameValueAndType(claim, claimToCheck));

    private static bool SameValueAndType(Claim? c1, Claim? c2)
    {
        if (c1 is null ||
            c2 is null)
            return false;

        return c1.ValueType.Equals(c2.ValueType) &&
            c1.Value.Equals(c2.Value);
    }

    private static int TryGetUserId(IEnumerable<Claim> claims)
    {
        var idClaimValue = TryGetFirstValue(claims, RoleClaim.DEFAULT_ID);
        if (!int.TryParse(idClaimValue, out int id))
            id = 0;
        return id;
    }

    private static string? TryGetFirstValue(IEnumerable<Claim> claims, string valueType)
        => TryGetValues(claims, valueType).FirstOrDefault();

    private static IEnumerable<string> TryGetValues(IEnumerable<Claim> claims, string valueType)
        => claims.Where(
            claim => claim.Type == valueType)
            .Select(claim => claim.Value);
}