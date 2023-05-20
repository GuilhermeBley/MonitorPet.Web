using Dapper;
using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Repositories;
using MonitorPet.Core.Entity;
using MonitorPet.Infrastructure.UoW;

namespace MonitorPet.Infrastructure.Repositories;

public class UsuarioDosadorRepository : RepositoryBase, IUsuarioDosadorRepository
{
    public UsuarioDosadorRepository(MySqlSession mysqlSession) : base(mysqlSession)
    {
    }

    public async Task<UsuarioDosadorModel> Create(UsuarioDosador entity)
    {
        return await _connection.QueryFirstAsync<UsuarioDosadorModel>(
            @"INSERT INTO monitorpet.dosadorusuario (IdUsuario, IdDosador) VALUES (@IdUsuario, @IdDosador);
            SELECT id Id, IdUsuario IdUsuario, IdDosador IdDosador FROM monitorpet.dosadorusuario
                WHERE Id = last_insert_id();",
            new { IdUsuario = entity.IdUser, IdDosador = entity.IdDosador },
            _transaction
        );
    }

    public async Task<UsuarioDosadorModel?> DeleteByIdOrDefault(int id)
    {
        return await _connection.QueryFirstAsync<UsuarioDosadorModel>(
            @"SELECT id Id, IdUsuario IdUsuario, IdDosador IdDosador FROM monitorpet.dosadorusuario
                WHERE Id = @Id;
            DELETE FROM monitorpet.dosadorusuario WHERE Id = @Id;",
            new { Id = id },
            _transaction
        );
    }

    public async Task<IEnumerable<UsuarioDosadorModel>> GetAll()
    {
        return await _connection.QueryAsync<UsuarioDosadorModel>(
            @"SELECT id Id, IdUsuario IdUsuario, IdDosador IdDosador FROM monitorpet.dosadorusuario;",
            _transaction
        );
    }

    public async Task<UsuarioDosadorModel?> GetByIdOrDefault(int id)
    {
        return await _connection.QueryFirstOrDefaultAsync<UsuarioDosadorModel>(
            @"SELECT id Id, IdUsuario IdUsuario, IdDosador IdDosador FROM monitorpet.dosadorusuario
                WHERE Id = @Id;",
            new { Id = id },
            _transaction
        );
    }

    public async Task<IEnumerable<DosadorJoinUsuarioDosadorModel>> GetByIdUser(int idUser)
    {
        return await _connection.QueryAsync<DosadorJoinUsuarioDosadorModel>(
            @"SELECT 
                ud.id Id, 
                ud.IdUsuario IdUsuario, 
                d.IdDosador IdDosador,
                d.ImgUrl ImgUrl,
                d.Nome Nome
            FROM monitorpet.dosadorusuario ud
            INNER JOIN monitorpet.dosador d
                ON d.IdDosador = ud.IdDosador
            WHERE ud.IdUsuario = @IdUsuario;",
            new { IdUsuario = idUser },
            _transaction
        );
    }

    public async Task<DosadorJoinUsuarioDosadorModel?> GetByIdUserAndIdDosador(int idUser, Guid idDosador)
    {
        return await _connection.QueryFirstOrDefaultAsync<DosadorJoinUsuarioDosadorModel>(
            @"SELECT 
                ud.id Id, 
                ud.IdUsuario IdUsuario, 
                d.IdDosador IdDosador,
                d.ImgUrl ImgUrl,
                d.Nome Nome
            FROM monitorpet.dosadorusuario ud
            INNER JOIN monitorpet.dosador d
                ON d.IdDosador = ud.IdDosador
            WHERE ud.IdUsuario = @IdUsuario
            AND ud.IdDosador = @IdDosador;",
            new { IdUsuario = idUser, IdDosador = idDosador },
            _transaction
        );
    }

    public async IAsyncEnumerable<JoinUsuarioDosadorInfoModel> GetInfoByIdUser(int idUser)
    {
        const int MAX_PER_PAGE = 10;

        IEnumerable<JoinUsuarioDosadorInfoModel> pageDosadores;
        int page = 0;

        do
        {
            pageDosadores = await _connection.QueryAsync<JoinUsuarioDosadorInfoModel>(
                @"SELECT 
	            ud.id Id, 
	            ud.IdUsuario IdUsuario, 
	            d.IdDosador IdDosador,
	            d.ImgUrl ImgUrl,
	            d.Nome Nome,
                (SELECT PesoGr FROM monitorpet.historicopeso his WHERE his.IdDosador = d.IdDosador ORDER BY DateAt DESC LIMIT 0,1) CurrentWeight,
                d.UltimaAtualizacao LastSeen,
                d.UltimaLiberacao LastRelease
            FROM monitorpet.dosadorusuario ud
            INNER JOIN monitorpet.dosador d
	            ON d.IdDosador = ud.IdDosador
            WHERE ud.IdUsuario = @IdUsuario
            LIMIT @Skip, @Take;",
                new { IdUsuario = idUser, Skip = page*MAX_PER_PAGE, Take = MAX_PER_PAGE },
                _transaction
            );

            foreach (var dosador in pageDosadores)
                yield return dosador;

            page++;

        } while (pageDosadores.Count() >= MAX_PER_PAGE);
    }

    public Task<UsuarioDosadorModel?> UpdateByIdOrDefault(int id, UsuarioDosador entity)
    {
        throw new NotImplementedException();
    }
}