using MonitorPet.Application.Model.Dosador;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Repositories;

public interface IUsuarioDosadorRepository : IRepositoryBase<UsuarioDosador, UsuarioDosadorModel, int>
{
    Task<IEnumerable<DosadorJoinUsuarioDosadorModel>> GetByIdUser(int idUser);
    IAsyncEnumerable<JoinUsuarioDosadorInfoModel> GetInfoByIdUser(int idUser);
    Task<DosadorJoinUsuarioDosadorModel?> GetByIdUserAndIdDosador(int idUser, Guid idDosador);
}