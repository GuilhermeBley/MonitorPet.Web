namespace MonitorPet.Application.Model.User;

public class QueryUserModel
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
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// NIckName
    /// </summary>
    public string? NickName { get; set; }
}