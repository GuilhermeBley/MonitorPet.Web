using MonitorPet.Application.Model.Agendamento;

namespace MonitorPet.Application.Tests.Mocks;

internal static class AgendamentoMock
{
    public static CreateAgendamentoModel CreateValidAgendamento(Guid idDosador, int weekDay = 1, TimeOnly timeOnly = default, double quantityReleased = 100)
    {
        return new CreateAgendamentoModel
        {
            DiaSemana = weekDay,
            HoraAgendada = timeOnly,
            IdDosador = idDosador,
            QtdeLiberadaGr = quantityReleased
        };
    }
}
