using MonitorPet.Application.Model.PesoHistorico;

namespace MonitorPet.Application.Services.Interfaces;

public interface IConsumptionService
{
    Task<ConsumptionIntervalModel> GetDay(Guid idDosador, DateTimeOffset start);
    Task<ConsumptionIntervalModel> GetWeekly(Guid idDosador, DateTimeOffset start);
}
