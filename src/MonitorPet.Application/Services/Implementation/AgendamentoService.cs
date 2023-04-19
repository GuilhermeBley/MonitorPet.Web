using MonitorPet.Application.Model.Agendamento;
using MonitorPet.Application.Repositories;
using MonitorPet.Application.Security;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Application.UoW;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Services.Implementation;

public class AgendamentoService : IAgendamentoService
{
    private readonly ContextClaim _contextClaim;
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly IUsuarioDosadorRepository _usuarioDosadorRepository;
    private readonly IUnitOfWork _uoW;

    public AgendamentoService(
        ContextClaim contextClaim,
        IAgendamentoRepository agendamentoRepository,
        IUnitOfWork uoW,
        IUsuarioDosadorRepository usuarioDosadorRepository)
    {
        _contextClaim = contextClaim;
        _agendamentoRepository = agendamentoRepository;
        _uoW = uoW;
        _usuarioDosadorRepository = usuarioDosadorRepository;
    }

    public async Task<AgendamentoModel> Create(CreateAgendamentoModel createAgendamentoModel)
    {
        var user = await _contextClaim.GetRequiredCurrentClaim();

        var entityToCreate = MapCreateAgendamentoModel(createAgendamentoModel);

        using var transaction = await _uoW.BeginTransactionAsync();

        await ThrowIfCannotAccessDosador(user.RequiredIdUser, createAgendamentoModel.IdDosador);

        var agendamentoCreated = await _agendamentoRepository.Create(entityToCreate);

        await transaction.SaveChangesAsync();

        return agendamentoCreated;
    }

    public async Task<AgendamentoModel> DeleteById(int id)
    {
        var user = await _contextClaim.GetRequiredCurrentClaim();

        if (id < 1)
            throw new Core.Exceptions.CommonCoreException("Id inválido.");

        using var transaction = await _uoW.BeginTransactionAsync();

        var agendamentoFound = await _agendamentoRepository.GetByIdOrDefault(id)
            ?? throw new Core.Exceptions.NotFoundCoreException("Agendamento não encontrado.");

        await ThrowIfCannotAccessDosador(user.RequiredIdUser, agendamentoFound.IdDosador);
        
        var agendamentoDeleted = await _agendamentoRepository.DeleteByIdOrDefault(agendamentoFound.Id)
            ?? throw new Core.Exceptions.NotFoundCoreException("Agendamento não encontrado.");

        await transaction.SaveChangesAsync();

        return agendamentoDeleted;
    }

    public async Task<IEnumerable<AgendamentoModel>> GetByDosador(string idDosador)
    {
        var user = await _contextClaim.GetRequiredCurrentClaim();

        if (!Guid.TryParse(idDosador, out Guid idDosadorGuid))
            throw new Core.Exceptions.CommonCoreException("Id inválido.");

        using var connection = await _uoW.OpenConnectionAsync();

        await ThrowIfCannotAccessDosador(user.RequiredIdUser, idDosadorGuid);

        return await _agendamentoRepository.GetByDosador(idDosadorGuid.ToString());
    }

    public async Task<AgendamentoModel> UpdateById(int id, CreateAgendamentoModel updateAgendamentoModel)
    {
        var user = await _contextClaim.GetRequiredCurrentClaim();

        if (id < 1)
            throw new Core.Exceptions.CommonCoreException("Id inválido.");

        var entityToUpdate = MapCreateAgendamentoModel(updateAgendamentoModel);

        using var transaction = await _uoW.BeginTransactionAsync();

        var agendamentoFound = await _agendamentoRepository.GetByIdOrDefault(id)
            ?? throw new Core.Exceptions.NotFoundCoreException("Agendamento não encontrado.");

        await ThrowIfCannotAccessDosador(user.RequiredIdUser, agendamentoFound.IdDosador);

        var agendamentoDeleted = await _agendamentoRepository.UpdateByIdOrDefault(agendamentoFound.Id, entityToUpdate)
            ?? throw new Core.Exceptions.NotFoundCoreException("Agendamento não encontrado.");

        await transaction.SaveChangesAsync();

        return agendamentoDeleted;
    }

    /// <summary>
    /// Needs a connection
    /// </summary>
    private async Task ThrowIfCannotAccessDosador(int idUser, Guid idDosador)
    {
        var dosadorFound =
            await _usuarioDosadorRepository.GetByIdUserAndIdDosador(idUser, idDosador);

        if (dosadorFound is null)
            throw new Core.Exceptions.ForbiddenCoreException("Usuário não tem acesso à dosador.");
    }

    private static Agendamento MapCreateAgendamentoModel(CreateAgendamentoModel create)
    {
        var dayOfWeek = TryGetDayOfWeek(create.DiaSemana)
            ?? throw new Core.Exceptions.CommonCoreException("Dia da semana inválido.");

        return Agendamento.CreateActivated(create.IdDosador, dayOfWeek, create.HoraAgendada, create.QtdeLiberadaGr);
    }

    private static DayOfWeek? TryGetDayOfWeek(int dayOfWeekInteger)
    {
        try
        {
            if (!Enum.IsDefined(typeof(DayOfWeek), dayOfWeekInteger))
                return null;
            return (DayOfWeek)dayOfWeekInteger;
        }
        catch
        {
            return null;
        }
    }
}
