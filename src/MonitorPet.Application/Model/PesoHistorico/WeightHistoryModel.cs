namespace MonitorPet.Application.Model.PesoHistorico;

public class WeightHistoryModel
{
    public long Id { get; set; }
    public long Weight { get; set; }
    public DateTimeOffset RegisteredAt { get; set; }
}
