using MonitorPet.Application.Email;
using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Repositories;
using MonitorPet.Application.Security;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Application.UoW;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Services.Implementation;

public class DosadorService : IDosadorService
{
    private readonly ContextClaim _contextClaim;
    private readonly IDosadorRepository _dosadorRepository;
    private readonly IUsuarioDosadorRepository _usuarioDosadorRepository;
    private readonly IUnitOfWork _uoW;

    public DosadorService(
        ContextClaim contextClaim,
        IDosadorRepository dosadorRepository,
        IUnitOfWork uoW,
        IUsuarioDosadorRepository usuarioDosadorRepository)
    {
        _contextClaim = contextClaim;
        _dosadorRepository = dosadorRepository;
        _uoW = uoW;
        _usuarioDosadorRepository = usuarioDosadorRepository;
    }

    public async Task<DosadorJoinUsuarioDosadorModel> AddDosadorToUser(int idUser, string token)
    {
        if (!Guid.TryParse(token, out Guid idDosador))
            throw new Core.Exceptions.CommonCoreException("Id dosador inválido.");

        var currentClaims = await _contextClaim.GetRequiredCurrentClaim();

        if (currentClaims.RequiredIdUser != idUser)
            throw new Core.Exceptions.UnauthorizedCoreException("Usuário não autorizado.");

        using var transaction = await _uoW.BeginTransactionAsync();

        bool alreadyContainsDosador = (await
            _usuarioDosadorRepository.GetByIdUserAndIdDosador(idUser, idDosador))
            is not null;

        if (alreadyContainsDosador)
            throw new Core.Exceptions.ConflictCoreException("Dosador já cadastrado.");

        var dosadorAdded = await
            _usuarioDosadorRepository.Create(Core.Entity.UsuarioDosador.Create(idDosador, idUser));

        var joinUserDosadorAdded = await
            _usuarioDosadorRepository.GetByIdUserAndIdDosador(idUser, idDosador)
            ?? throw new Core.Exceptions.NotFoundCoreException("Dosador não encontrado.");

        await transaction.SaveChangesAsync();

        return joinUserDosadorAdded;
    }

    public async Task<DosadorJoinUsuarioDosadorModel> RemoveDosadorFromUser(int idUser, Guid idDosador)
    {
        if (idDosador == default)
            throw new Core.Exceptions.CommonCoreException("Id do dosador é inválido.");

        var currentClaims = await _contextClaim.GetRequiredCurrentClaim();

        if (currentClaims.RequiredIdUser != idUser)
            throw new Core.Exceptions.UnauthorizedCoreException("Usuário não autorizado.");

        using var transaction = await _uoW.BeginTransactionAsync();

        bool containsDosador = (await
            _usuarioDosadorRepository.GetByIdUserAndIdDosador(idUser, idDosador))
            is not null;

        if (!containsDosador)
            throw new Core.Exceptions.NotFoundCoreException("Dosador não encontrado.");

        var joinUserDosadorToRemove = await
            _usuarioDosadorRepository.GetByIdUserAndIdDosador(idUser, idDosador)
            ?? throw new Core.Exceptions.NotFoundCoreException("Dosador não encontrado.");

        var dosadorAdded = await
            _usuarioDosadorRepository.DeleteByIdOrDefault(joinUserDosadorToRemove.Id);

        await transaction.SaveChangesAsync();

        return joinUserDosadorToRemove;
    }

    public async Task<IEnumerable<DosadorJoinUsuarioDosadorModel>> GetDosadoresByIdUser(int idUser)
    {
        var currentClaims = await _contextClaim.GetRequiredCurrentClaim();

        if (currentClaims.RequiredIdUser != idUser)
            throw new Core.Exceptions.UnauthorizedCoreException("Usuário não autorizado.");

        using var connection = await _uoW.OpenConnectionAsync();

        return await _usuarioDosadorRepository.GetByIdUser(idUser);
    }

    public async Task<DosadorModel> UpdateNameDosador(int idUser, Guid idDosador, string newName)
    {
        if (idDosador == default)
            throw new Core.Exceptions.CommonCoreException("Id do dosador é inválido.");

        var currentClaims = await _contextClaim.GetRequiredCurrentClaim();

        if (currentClaims.RequiredIdUser != idUser)
            throw new Core.Exceptions.UnauthorizedCoreException("Usuário não autorizado.");

        using var transaction = await _uoW.BeginTransactionAsync();

        bool userContainsDosador = (await
            _usuarioDosadorRepository.GetByIdUserAndIdDosador(idUser, idDosador))
            is not null;

        if (!userContainsDosador)
            throw new Core.Exceptions.NotFoundCoreException("Dosador não encontrado.");

        var entityToUpdate = Core.Entity.Dosador.CreateWithoutImg(newName);

        var dosadorUpdated = await
            _dosadorRepository.UpdateByIdOrDefault(idDosador, entityToUpdate)
            ?? throw new Core.Exceptions.NotFoundCoreException("Dosador não encontrado.");

        await transaction.SaveChangesAsync();

        return dosadorUpdated;
    }
}
