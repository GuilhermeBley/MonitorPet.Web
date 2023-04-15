using MonitorPet.Application.Model.Dosador;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Repositories;

public interface IDosadorRepository : IRepositoryBase<Dosador, DosadorModel, Guid>
{
    
}