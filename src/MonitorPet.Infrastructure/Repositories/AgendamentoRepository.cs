using Dapper;
using MonitorPet.Application.Model.Agendamento;
using MonitorPet.Application.Repositories;
using MonitorPet.Core.Entity;
using MonitorPet.Infrastructure.UoW;

namespace MonitorPet.Infrastructure.Repositories;

internal class AgendamentoRepository : RepositoryBase, IAgendamentoRepository
{
    public AgendamentoRepository(MySqlSession mysqlSession) : base(mysqlSession)
    {
    }

    public async Task<AgendamentoModel> Create(Agendamento entity)
    {
        return await _connection
            .QueryFirstOrDefaultAsync<AgendamentoModel>(
            @"INSERT INTO monitorpet.agendamento (IdDosador, DiaSemana, HoraAgendada, QtdeLiberadaGr, Ativado) 
                VALUES (@IdDosador, @DiaSemana, @HoraAgendada, @QtdeLiberadaGr, @Ativado);
            SELECT Id, IdDosador, DiaSemana, HoraAgendada, QtdeLiberadaGr, Ativado from monitorpet.agendamento
                WHERE Id=last_insert_id();",
            entity,
            _transaction);
    }

    public async Task<AgendamentoModel?> DeleteByIdOrDefault(int id)
    {
        return await _connection
            .QueryFirstOrDefaultAsync<AgendamentoModel>(
            @"SELECT Id, IdDosador, DiaSemana, HoraAgendada, QtdeLiberadaGr, Ativado from monitorpet.agendamento
                WHERE Id=@Id;
            DELETE FROM monitorpet.agendamento WHERE (Id = @Id);",
            new { Id = id },
            _transaction);
    }

    public async Task<IEnumerable<AgendamentoModel>> GetAll()
    {
        return await _connection
            .QueryAsync<AgendamentoModel>(
            @"SELECT Id, IdDosador, DiaSemana, HoraAgendada, QtdeLiberadaGr, Ativado from monitorpet.agendamento;",
            _transaction);
    }

    public async Task<IEnumerable<AgendamentoModel>> GetByDosador(Guid idDosador)
    {
        return await _connection
            .QueryAsync<AgendamentoModel>(
            @"SELECT Id, IdDosador, DiaSemana, HoraAgendada, QtdeLiberadaGr, Ativado from monitorpet.agendamento
                WHERE IdDosador=@IdDosador;",
            new { IdDosador = idDosador },
            _transaction);
    }

    public async Task<AgendamentoModel?> GetByIdOrDefault(int id)
    {
        return await _connection
            .QueryFirstOrDefaultAsync<AgendamentoModel>(
            @"SELECT Id, IdDosador, DiaSemana, HoraAgendada, QtdeLiberadaGr, Ativado from monitorpet.agendamento
                WHERE Id=@Id;",
            new { Id = id },
            _transaction);
    }

    public async Task<AgendamentoModel?> UpdateByIdOrDefault(int id, Agendamento entity)
    {
        return await _connection
            .QueryFirstOrDefaultAsync<AgendamentoModel>(
            @"UPDATE monitorpet.agendamento SET IdDosador = @IdDosador, DiaSemana = @DiaSemana, 
                HoraAgendada = @HoraAgendada, QtdeLiberadaGr = @QtdeLiberadaGr, Ativado = @Ativado WHERE (Id = @Id);
            SELECT Id, IdDosador, DiaSemana, HoraAgendada, QtdeLiberadaGr, Ativado from monitorpet.agendamento
                WHERE Id=@Id;",
            new { Id = id, entity.IdDosador, entity.DiaSemana, entity.HoraAgendada, entity.QtdeLiberadaGr,
                entity.Ativado },
            _transaction
            );
    }
}
