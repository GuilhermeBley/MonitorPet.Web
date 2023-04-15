using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.User;

public class UpdatePasswordViewModel
{
    [Required(ErrorMessage = "Senha antiga é obrigatória.")]
    public string OldPassword { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Nova senha é obrigatória.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{8,100}$",
        ErrorMessage = "Senha deve conter 8 caracteres, sendo no mínimo um caracter especial, um número e uma letra maiúscula e minúscula.")]
    public string NewPassword { get; set; } = string.Empty;

    [Compare("NewPassword", ErrorMessage = "Senhas não conferem.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
