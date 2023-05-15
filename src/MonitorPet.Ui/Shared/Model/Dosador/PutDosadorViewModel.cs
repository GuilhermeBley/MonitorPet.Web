using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.Dosador;

public class PutDosadorViewModel
{
    [Required]
    public string NewName { get; set; } = string.Empty;
    public bool UpdateImg { get; set; } = false;
    public byte[] NewImage { get; set; } = new byte[0];
}
