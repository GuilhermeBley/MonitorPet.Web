using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.User;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string EmailAddress { get; set; } = string.Empty;
}

