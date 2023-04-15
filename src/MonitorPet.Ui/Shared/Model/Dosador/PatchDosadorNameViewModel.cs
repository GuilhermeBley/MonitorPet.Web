using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.Dosador;

public class PatchDosadorNameViewModel
{
    [Required]
    public string NewName { get; set; } = string.Empty; 
}
