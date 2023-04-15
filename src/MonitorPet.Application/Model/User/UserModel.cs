namespace MonitorPet.Application.Model.User;

public class UserModel
{
    /// <summary>
    /// Identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Email confirmed
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Date until lockout
    /// </summary>
    public DateTime? LockOutEnd { get; set; }

    /// <summary>
    /// Count of fails access
    /// </summary>
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// NIckName
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
}