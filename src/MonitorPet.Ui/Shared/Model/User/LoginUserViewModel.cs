using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.User;

public class LoginUserViewModel
{
    /// <summary>
    /// Email
    /// </summary>
    [Required(ErrorMessage = "Login é obrigatório.")]
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Senha é obrigatória.")]
    public string Password { get; set; } = string.Empty;
}