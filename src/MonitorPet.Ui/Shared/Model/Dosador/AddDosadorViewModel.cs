using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.Dosador;

public class AddDosadorViewModel
{
    [Required]
    public string Token { get; set; } = string.Empty;
}
