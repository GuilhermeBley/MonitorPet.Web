namespace MonitorPet.Application.Model.Dosador;

public class UpdateDosadorModel
{
    public string Name { get; set; } = string.Empty;
    public Stream ImgStream { get; set; } = Stream.Null;
    public bool UpdateImg { get; set; }
}
