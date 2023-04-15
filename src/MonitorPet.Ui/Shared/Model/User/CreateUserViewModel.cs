using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.User;

public class CreateUserViewModel
{
    /// <summary>
    /// Email
    /// </summary>
    [Required(ErrorMessage = "Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Name
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [StringLength(255, MinimumLength = 4, ErrorMessage = "Nome deve ter no mínimo 4 caracteres.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// NIckName
    /// </summary>
    [StringLength(255, MinimumLength = 4, ErrorMessage = "Apelido deve ter no mínimo 4 caracteres.")]
    public string? NickName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Senha é obrigatória.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{8,100}$", 
        ErrorMessage = "Senha deve conter 8 caracteres, sendo no mínimo um caracter especial, um número e uma letra maiúscula e minúscula.")]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "Senhas não conferem.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// Token to access dosador
    /// </summary>
    [Required(ErrorMessage = "Token de acesso ao dosador é obrigatório.")]
    public string? TokenAccessDosador { get; set; }
}