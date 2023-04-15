using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.User;

public class UpdateUserViewModel
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
}
