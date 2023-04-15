using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Repositories;
using MonitorPet.Application.Tests.InMemoryDb;
using MonitorPet.Application.Tests.ModelDb;
using MonitorPet.Application.Tests.UoW;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Tests.Repositories;

internal class UsuarioDosadorRepository : RepositoryBase, IUsuarioDosadorRepository
{
    public UsuarioDosadorRepository(IMemorySession memorySession, IMapper mapper) 
        : base(memorySession, mapper)
    {
    }

    public async Task<UsuarioDosadorModel> Create(UsuarioDosador entity)
    {
        var userDosadorDb = _mapper.Map<UsuarioDosadorDbModel>(entity);

        await _context.UsuariosDosadores.AddAsync(userDosadorDb);
        await _context.SaveChangesAsync();

        return _mapper.Map<UsuarioDosadorModel>(userDosadorDb);
    }

    public async Task<UsuarioDosadorModel?> DeleteByIdOrDefault(int id)
    {
        var userDosadorDb = await _context.UsuariosDosadores.FirstOrDefaultAsync(u => u.Id == id);

        if (userDosadorDb is null)
            return null;

        _context.UsuariosDosadores.Remove(userDosadorDb);

        await _context.SaveChangesAsync();

        return _mapper.Map<UsuarioDosadorModel>(userDosadorDb);
    }

    public async Task<IEnumerable<UsuarioDosadorModel>> GetAll()
        => (await _context.UsuariosDosadores.AsNoTracking().ToListAsync())
            .Select(userDosadorDb => _mapper.Map<UsuarioDosadorModel>(userDosadorDb));

    public async Task<UsuarioDosadorModel?> GetByIdOrDefault(int id)
    {
        var usuarioDosadorDb = await _context.UsuariosDosadores.FirstOrDefaultAsync(u => u.Id == id);

        if (usuarioDosadorDb is null)
            return null;

        return _mapper.Map<UsuarioDosadorModel>(usuarioDosadorDb);
    }

    public async Task<IEnumerable<DosadorJoinUsuarioDosadorModel>> GetByIdUser(int idUser)
        => await (from userDosadorDb in _context.UsuariosDosadores.AsNoTracking()
            join dosador in _context.Dosadores.AsNoTracking()
            on userDosadorDb.IdDosador equals dosador.IdDosador
            where userDosadorDb.IdUsuario.Equals(idUser)
            select new DosadorJoinUsuarioDosadorModel
            {
                Id = userDosadorDb.Id,
                IdDosador = userDosadorDb.IdDosador,
                IdUsuario = userDosadorDb.IdUsuario,
                Nome = dosador.Nome,
                PesoMax = dosador.PesoMax
            }).ToListAsync();

    public async Task<DosadorJoinUsuarioDosadorModel?> GetByIdUserAndIdDosador(int idUser, Guid idDosador)
        => await (from userDosadorDb in _context.UsuariosDosadores.AsNoTracking()
            join dosador in _context.Dosadores.AsNoTracking()
            on userDosadorDb.IdDosador equals dosador.IdDosador
            where userDosadorDb.IdUsuario.Equals(idUser)
            where dosador.IdDosador.Equals(idDosador)
            select new DosadorJoinUsuarioDosadorModel
            {
                Id = userDosadorDb.Id,
                IdDosador = userDosadorDb.IdDosador,
                IdUsuario = userDosadorDb.IdUsuario,
                Nome = dosador.Nome,
                PesoMax = dosador.PesoMax
            }).FirstOrDefaultAsync();

    public async Task<UsuarioDosadorModel?> UpdateByIdOrDefault(int id, UsuarioDosador entity)
    {
        var userDosadorDb = await _context.UsuariosDosadores.FirstOrDefaultAsync(u => u.Id == id);

        if (userDosadorDb is null)
            return null;

        var userDosadorToUpdate = _mapper.Map<UsuarioDosadorDbModel>(entity);
        userDosadorDb.IdDosador = userDosadorToUpdate.IdDosador;
        userDosadorDb.IdUsuario = userDosadorToUpdate.IdUsuario;

        _context.UsuariosDosadores.Update(userDosadorDb);

        await _context.SaveChangesAsync();

        return _mapper.Map<UsuarioDosadorModel>(
            await _context.UsuariosDosadores.FirstOrDefaultAsync(u => u.Id == id)
        );
    }
}