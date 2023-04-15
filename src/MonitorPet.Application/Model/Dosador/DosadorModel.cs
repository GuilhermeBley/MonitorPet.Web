namespace MonitorPet.Application.Model.Dosador;

public class DosadorModel
{
    public Guid IdDosador { get; set; }
    public string Nome { get; set; } = string.Empty;
    public double PesoMax { get; set; }
}