namespace MonitorPet.Application.Model.PesoHistorico;

public class WeightHistoryModel
{
    public long Id { get; set; }
    public double Weight { get; set; }
    public DateTimeOffset RegisteredAt { get; set; }
}
