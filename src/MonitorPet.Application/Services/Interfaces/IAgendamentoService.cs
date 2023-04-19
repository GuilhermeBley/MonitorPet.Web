using MonitorPet.Application.Model.Agendamento;
using MonitorPet.Core.Exceptions;

namespace MonitorPet.Application.Services.Interfaces;

public interface IAgendamentoService
{
    /// <summary>
    /// Create new agendamento
    /// </summary>
    /// <exception cref="CoreException"></exception>
    Task<AgendamentoModel> Create(CreateAgendamentoModel createAgendamentoModel);

    /// <summary>
    /// Delete by id own agendamento
    /// </summary>
    /// <remarks>
    ///     <para>Must have authorization to delete.</para>
    /// </remarks>
    /// <exception cref="ForbiddenCoreException"></exception>
    /// <exception cref="CoreException"></exception>
    Task<AgendamentoModel> DeleteById(int id);

    /// <summary>
    /// Get agendamentos
    /// </summary>
    /// <exception cref="CoreException"></exception>
    Task<IEnumerable<AgendamentoModel>> GetByDosador(string idDosador);

    /// <summary>
    /// Update by id own agendamento
    /// </summary>
    /// <remarks>
    ///     <para>Must have authorization to update.</para>
    /// </remarks>
    /// <exception cref="ForbiddenCoreException"></exception>
    /// <exception cref="CoreException"></exception>
    Task<AgendamentoModel> UpdateById(int id, CreateAgendamentoModel updateAgendamentoModel);
}
