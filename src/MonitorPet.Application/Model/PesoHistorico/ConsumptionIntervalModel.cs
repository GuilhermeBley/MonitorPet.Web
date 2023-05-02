namespace MonitorPet.Application.Model.PesoHistorico;

public class ConsumptionIntervalModel
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public IReadOnlyList<ConsumptionModel> Consumptions { get; set; }
        = new List<ConsumptionModel>();
    public DateTimeOffset GeneratedAt { get; set; }
}
