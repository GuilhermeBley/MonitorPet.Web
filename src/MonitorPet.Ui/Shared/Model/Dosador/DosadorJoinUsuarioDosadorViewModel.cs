namespace MonitorPet.Ui.Shared.Model.Dosador;

public class DosadorJoinUsuarioDosadorViewModel
{
    public int Id { get; set; }
    public Guid IdDosador { get; set; }
    public int IdUsuario { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? ImgUrl { get; set; }
}
