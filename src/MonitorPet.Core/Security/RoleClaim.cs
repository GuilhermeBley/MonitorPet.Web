using System.Security.Claims;

namespace MonitorPet.Core.Security;

public static class RoleClaim
{
    public const string DEFAULT_EMAIL_TYPE = "EmailAddress";
    public const string DEFAULT_ID = "Id";
    public const string DEFAULT_ID_DOSADOR = "IdDosador";
    public const string DEFAULT_NAME = "Name";
    public const string DEFAULT_ROLE_TYPE = "Role";

    public static Claim RoleConfirmEmail
        => new(DEFAULT_ROLE_TYPE, "ConfirmEmail");

    public static Claim RoleChangePassword
        => new(DEFAULT_ROLE_TYPE, "ChangePassword");

    /// <summary>
    /// name | <paramref name="name"/>
    /// </summary>
    public static Claim CreateUserClaimName(string name)
        => new Claim(DEFAULT_NAME, name);

    /// <summary>
    /// id | <paramref name="id"/>
    /// </summary>
    public static Claim CreateUserClaimId(string id)
        => new Claim(DEFAULT_ID, id);

    /// <summary>
    /// emailaddress | <paramref name="email"/>
    /// </summary>
    public static Claim CreateUserClaimEmail(string email)
        => new Claim(DEFAULT_EMAIL_TYPE, email);

    /// <summary>
    /// nameidentifier | <paramref name="idDosador"/>
    /// </summary>
    public static Claim CreateIdDosador(string idDosador)
        => new Claim(DEFAULT_ID_DOSADOR, idDosador);
} 