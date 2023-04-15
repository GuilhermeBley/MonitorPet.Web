using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.User
{
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Senha é obrigatória.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{8,100}$",
            ErrorMessage = "Senha deve conter 8 caracteres, sendo no mínimo um caracter especial, um número e uma letra maiúscula e minúscula.")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Senhas não conferem.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
