namespace MonitorPet.Ui.Shared.Model.PesoHistorico;

public class ConsumptionIntervalViewModel
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public IReadOnlyList<ConsumptionViewModel> Consumptions { get; set; }
        = new List<ConsumptionViewModel>();
    public DateTimeOffset GeneratedAt { get; set; }
}
