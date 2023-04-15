namespace MonitorPet.Application.Model.User;

public class CreateUserModel
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;

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

    /// <summary>
    /// Token to access dosador
    /// </summary>
    public string? TokenAccessDosador { get; set; }
}