using MonitorPet.Application.Model.PesoHistorico;

namespace MonitorPet.Application.Services.Interfaces;

public interface IConsumptionService
{
    Task<ConsumptionIntervalModel> GetDaily(Guid idDosador, DateTimeOffset start);
    Task<ConsumptionIntervalModel> GetWeekly(Guid idDosador, DateTimeOffset start);
}
