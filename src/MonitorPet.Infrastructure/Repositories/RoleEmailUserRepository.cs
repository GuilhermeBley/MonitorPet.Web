using Dapper;
using MonitorPet.Application.Model.Email;
using MonitorPet.Application.Repositories;
using MonitorPet.Core.Entity;
using MonitorPet.Infrastructure.UoW;
using static Dapper.SqlMapper;

namespace MonitorPet.Infrastructure.Repositories;

internal class RoleEmailUserRepository : RepositoryBase, IRoleEmailUserRepository
{
    public RoleEmailUserRepository(MySqlSession mysqlSession) : base(mysqlSession)
    {
    }

    public async Task<RoleEmailUserModel> Create(RoleEmailUser entity)
        => await _connection.QueryFirstOrDefaultAsync<RoleEmailUserModel>(
            @"INSERT INTO monitorpet.regraemailuser (IdUsuario, IdTipoEmail) VALUES (@UserId, @EmailTypeId);" +
            @"SELECT Id Id, IdUsuario UserId, IdTipoEmail EmailTypeId FROM monitorpet.regraemailuser
WHERE Id=last_insert_id();",
            new { UserId = entity.UserId, EmailTypeId = entity.Role.Id },
            transaction: _transaction
        );

    public async Task<RoleEmailUserModel?> DeleteByIdOrDefault(int id)
        => await _connection.QueryFirstOrDefaultAsync<RoleEmailUserModel>(
            @"SELECT Id Id, IdUsuario UserId, IdTipoEmail EmailTypeId FROM monitorpet.regraemailuser;
WHERE Id=@Id;" +
            @"DELETE FROM monitorpet.regraemailuser WHERE Id=@Id;",
            new { Id = id },
            transaction: _transaction
        );

    public async Task<int> DeleteByIdUser(int idUser)
        => await _connection.ExecuteAsync(
                @"DELETE FROM monitorpet.regraemailuser WHERE IdUsuario=@IdUsuario;",
                new { IdUsuario = idUser },
                transaction: _transaction
            );

    public Task<IEnumerable<RoleEmailUserModel>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<RoleEmailUserModel?> GetByIdOrDefault(int id)
        => await _connection.QueryFirstOrDefaultAsync<RoleEmailUserModel>(
            @"SELECT Id Id, IdUsuario UserId, IdTipoEmail EmailTypeId FROM monitorpet.regraemailuser;
WHERE Id=@Id;",
            new { Id = id },
            transaction: _transaction
        );

    public async Task<IEnumerable<QueryRoleEmailUserModel>> GetByIdUser(int idUser)
        => await _connection.QueryAsync<QueryRoleEmailUserModel>(
                @"
SELECT 
	r.Id Id, 
    r.IdUsuario UserId, 
    r.IdTipoEmail EmailTypeId,
    t.TipoEnvio Type, 
    t.Descricao Description
FROM monitorpet.regraemailuser r
INNER JOIN monitorpet.tipoemail t
	ON r.IdTipoEmail = t.Id
WHERE IdUsuario=@IdUsuario;",
                new { IdUsuario = idUser },
                transaction: _transaction
            );

    public Task<RoleEmailUserModel?> UpdateByIdOrDefault(int id, RoleEmailUser entity)
    {
        throw new NotImplementedException();
    }
}
