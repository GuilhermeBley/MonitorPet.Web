using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Repositories;
using MonitorPet.Application.Tests.InMemoryDb;
using MonitorPet.Application.Tests.ModelDb;
using MonitorPet.Application.Tests.UoW;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Tests.Repositories;

internal class DosadorRepository : RepositoryBase, IDosadorRepository
{
    public DosadorRepository(IMemorySession memorySession, IMapper mapper) 
        : base(memorySession, mapper)
    {
    }

    public async Task<DosadorModel> Create(Dosador entity)
    {
        var dosadorDbModel = _mapper.Map<DosadorDbModel>(entity);

        await _context.Dosadores.AddAsync(dosadorDbModel);
        await _context.SaveChangesAsync();

        return _mapper.Map<DosadorModel>(dosadorDbModel);
    }

    public async Task<DosadorModel?> DeleteByIdOrDefault(Guid id)
    {
        var dosadorDb = await _context.Dosadores.FirstOrDefaultAsync(u => u.IdDosador == id);

        if (dosadorDb is null)
            return null;

        _context.Dosadores.Remove(dosadorDb);

        await _context.SaveChangesAsync();

        return _mapper.Map<DosadorModel>(dosadorDb);
    }

    public async Task<IEnumerable<DosadorModel>> GetAll()
        => (await _context.Dosadores.AsNoTracking().ToListAsync())
            .Select(table => _mapper.Map<DosadorModel>(table));

    public async Task<DosadorModel?> GetByIdOrDefault(Guid id)
    {
        var dosadorDb = await _context.Dosadores.FirstOrDefaultAsync(u => u.IdDosador == id);

        if (dosadorDb is null)
            return null;

        return _mapper.Map<DosadorModel>(dosadorDb);
    }

    public async Task<DosadorModel?> UpdateByIdOrDefault(Guid id, Dosador entity)
    {
        var dosadorDb = await _context.Dosadores.FirstOrDefaultAsync(u => u.IdDosador == id);

        if (dosadorDb is null)
            return null;

        var dosadorToUpdate = _mapper.Map<DosadorDbModel>(entity);
        dosadorDb.Nome = dosadorToUpdate.Nome;
        dosadorDb.ImgUrl = dosadorToUpdate.ImgUrl;
        
        _context.Dosadores.Update(dosadorDb);

        await _context.SaveChangesAsync();

        return _mapper.Map<DosadorModel>(
            await _context.Dosadores.FirstOrDefaultAsync(u => u.IdDosador == id)
        );
    }
}