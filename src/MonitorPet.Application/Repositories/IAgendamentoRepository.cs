using MonitorPet.Application.Model.Agendamento;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Repositories;

public interface IAgendamentoRepository : IRepositoryBase<Agendamento, AgendamentoModel, int>
{
    Task<IEnumerable<AgendamentoModel>> GetByDosador(string idDosador);
}
