using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonitorPet.Application.Model.Agendamento;
using MonitorPet.Application.Repositories;
using MonitorPet.Application.Tests.ModelDb;
using MonitorPet.Application.Tests.UoW;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Tests.Repositories;

internal class AgendamentoRepository : RepositoryBase, IAgendamentoRepository
{
    public AgendamentoRepository(IMemorySession memorySession, IMapper mapper) : base(memorySession, mapper)
    {
    }


    public async Task<AgendamentoModel> Create(Agendamento entity)
    {
        var agendamentoDbModel = _mapper.Map<AgendamentoDbModel>(entity);

        await _context.Agendamentos.AddAsync(agendamentoDbModel);
        await _context.SaveChangesAsync();

        return _mapper.Map<AgendamentoModel>(agendamentoDbModel);
    }

    public async Task<AgendamentoModel?> DeleteByIdOrDefault(int id)
    {
        var agendamentoDb = await _context.Agendamentos.FirstOrDefaultAsync(u => u.Id == id);

        if (agendamentoDb is null)
            return null;

        _context.Agendamentos.Remove(agendamentoDb);

        await _context.SaveChangesAsync();

        return _mapper.Map<AgendamentoModel>(agendamentoDb);
    }

    public async Task<IEnumerable<AgendamentoModel>> GetAll()
        => (await _context.Agendamentos.AsNoTracking().ToListAsync())
            .Select(table => _mapper.Map<AgendamentoModel>(table));

    public async Task<IEnumerable<AgendamentoModel>> GetByDosador(Guid idDosador)
        => await _context.Agendamentos
            .Where(a => a.IdDosador == idDosador)
            .Select(agendamentoDb => _mapper.Map<AgendamentoModel>(agendamentoDb))
            .ToListAsync();

    public async Task<AgendamentoModel?> GetByIdOrDefault(int id)
    {
        var agendamentoDb = await _context.Agendamentos.FirstOrDefaultAsync(u => u.Id == id);

        if (agendamentoDb is null)
            return null;

        return _mapper.Map<AgendamentoModel>(agendamentoDb);
    }

    public async Task<AgendamentoModel?> UpdateByIdOrDefault(int id, Agendamento entity)
    {
        var agendamento = await _context.Agendamentos.FirstOrDefaultAsync(u => u.Id == id);

        if (agendamento is null)
            return null;

        var agendamentoToUpdate = _mapper.Map<AgendamentoDbModel>(entity);
        agendamento.QtdeLiberadaGr = agendamentoToUpdate.QtdeLiberadaGr;
        agendamento.HoraAgendada = agendamentoToUpdate.HoraAgendada;
        agendamento.IdDosador = agendamentoToUpdate.IdDosador;
        agendamento.Ativado = agendamentoToUpdate.Ativado;
        agendamento.DiaSemana = agendamentoToUpdate.DiaSemana;

        _context.Agendamentos.Update(agendamento);

        await _context.SaveChangesAsync();

        return _mapper.Map<AgendamentoModel>(
            await _context.Agendamentos.FirstOrDefaultAsync(u => u.Id == id)
        );
    }
}
