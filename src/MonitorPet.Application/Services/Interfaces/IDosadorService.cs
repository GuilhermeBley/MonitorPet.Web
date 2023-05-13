using MonitorPet.Application.Model.Dosador;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Services.Interfaces;

public interface IDosadorService
{
    Task<IEnumerable<DosadorJoinUsuarioDosadorModel>> GetDosadoresByIdUser(int idUser);
    Task<DosadorJoinUsuarioDosadorModel> AddDosadorToUser(int idUser, string token);
    Task<DosadorJoinUsuarioDosadorModel> RemoveDosadorFromUser(int idUser, Guid idDosador);
    Task<DosadorModel> UpdateNameDosador(int idUser, Guid idDosador, string newName);
    Task<DosadorModel> UpdateDosador(int idUser, Guid idDosador, UpdateDosadorModel updateDosadorModel);
}
