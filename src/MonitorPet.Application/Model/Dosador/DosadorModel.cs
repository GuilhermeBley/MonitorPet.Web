namespace MonitorPet.Application.Model.Dosador;

public class DosadorModel
{
    public Guid IdDosador { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? ImgUrl { get; set; }
    public DateTime? LastRefresh { get; set; }
    public DateTime? LastRelease { get; set; }
}