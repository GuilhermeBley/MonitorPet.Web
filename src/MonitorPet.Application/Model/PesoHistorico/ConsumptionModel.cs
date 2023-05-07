namespace MonitorPet.Application.Model.PesoHistorico;

public class ConsumptionModel
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public double QttConsumption { get; set; }
}
