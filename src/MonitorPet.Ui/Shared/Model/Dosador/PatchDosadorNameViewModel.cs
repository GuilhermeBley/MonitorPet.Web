using System.ComponentModel.DataAnnotations;

namespace MonitorPet.Ui.Shared.Model.Dosador;

public class PutDosadorNameViewModel
{
    [Required]
    public string NewName { get; set; } = string.Empty;
    public bool UpdateImg { get; set; } = false;
    public Stream NewImage { get; set; } = Stream.Null;
}
