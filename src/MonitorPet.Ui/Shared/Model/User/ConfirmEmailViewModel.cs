using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.User;

public class ConfirmEmailViewModel
{
    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;
}
