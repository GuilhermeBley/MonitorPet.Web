using Dapper;
using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Model.User;
using MonitorPet.Application.Repositories;
using MonitorPet.Core.Entity;
using MonitorPet.Infrastructure.UoW;

namespace MonitorPet.Infrastructure.Repositories;

public class UserRepository : RepositoryBase, IUserRepository
{
    public UserRepository(MySqlSession mysqlSession) : base(mysqlSession)
    {
    }

    public async Task<UserModel> Create(User entity)
    {
        return await _connection.QueryFirstAsync<UserModel>(
            @"INSERT INTO monitorpet.usuario (Nome, Apelido, Email, EmailConfirmado, Login, SenhaHash, SenhaSalt, BloqueadoAte, ContagemErros) 
	            VALUES (@Name, @NickName, @Email, @EmailConfirmed, @Login, @PasswordHash, @PasswordSalt, @LockOutEnd, @AccessFailedCount);
            SELECT IdUsuario Id, Nome Name, Apelido NickName, Email Email, EmailConfirmado EmailConfirmed,
                Login Login, SenhaHash PasswordHash, SenhaSalt PasswordSalt, BloqueadoAte LockOutEnd, 
                ContagemErros AccessFailedCount FROM monitorpet.usuario
                WHERE IdUsuario = last_insert_id();",
            entity,
            _transaction
        );
    }

    public async Task<UserModel?> DeleteByIdOrDefault(int id)
    {
        return await _connection.QueryFirstOrDefaultAsync<UserModel>(
            @"SELECT IdUsuario Id, Nome Name, Apelido NickName, Email Email, EmailConfirmado EmailConfirmed,
                Login Login, SenhaHash PasswordHash, SenhaSalt PasswordSalt, BloqueadoAte LockOutEnd, 
                ContagemErros AccessFailedCount FROM monitorpet.usuario
                WHERE IdUsuario = @Id;
            DELETE FROM monitorpet.usuario WHERE IdUsuario = @Id;",
            new { Id = id },
            _transaction
        );
    }

    public async Task<IEnumerable<UserModel>> GetAll()
    {
        return await _connection.QueryAsync<UserModel>(
            @"SELECT IdUsuario Id, Nome Name, Apelido NickName, Email Email, EmailConfirmado EmailConfirmed,
                Login Login, SenhaHash PasswordHash, SenhaSalt PasswordSalt, BloqueadoAte LockOutEnd, 
                ContagemErros AccessFailedCount FROM monitorpet.usuario;",
            _transaction
        );
    }

    public async Task<UserModel?> GetByEmailOrDefault(string email)
    {
        return await _connection.QueryFirstOrDefaultAsync<UserModel>(
            @"SELECT IdUsuario Id, Nome Name, Apelido NickName, Email Email, EmailConfirmado EmailConfirmed,
                Login Login, SenhaHash PasswordHash, SenhaSalt PasswordSalt, BloqueadoAte LockOutEnd, 
                ContagemErros AccessFailedCount FROM monitorpet.usuario
                WHERE Email = @Email;",
            new { Email = email },
            _transaction
        );
    }

    public async Task<UserModel?> GetByIdOrDefault(int id)
    {
        return await _connection.QueryFirstOrDefaultAsync<UserModel>(
            @"SELECT IdUsuario Id, Nome Name, Apelido NickName, Email Email, EmailConfirmado EmailConfirmed,
                Login Login, SenhaHash PasswordHash, SenhaSalt PasswordSalt, BloqueadoAte LockOutEnd, 
                ContagemErros AccessFailedCount FROM monitorpet.usuario
                WHERE IdUsuario = @Id;",
            new { Id = id },
            _transaction
        );
    }

    public async Task<UserModel?> UpdateAccessAccountFailed(int id, User entity)
    {
        return await _connection.QueryFirstOrDefaultAsync<UserModel>(
            @"UPDATE monitorpet.usuario SET BloqueadoAte = @LockOutEnd, 
                ContagemErros = @AccessFailedCount WHERE (IdUsuario = @Id);
            SELECT IdUsuario Id, Nome Name, Apelido NickName, Email Email, EmailConfirmado EmailConfirmed,
                Login Login, SenhaHash PasswordHash, SenhaSalt PasswordSalt, BloqueadoAte LockOutEnd, 
                ContagemErros AccessFailedCount FROM monitorpet.usuario
                WHERE IdUsuario = @Id;",
            new { Id = id, entity.AccessFailedCount , entity.LockOutEnd },
            _transaction
        );
    }

    public async Task<UserModel?> UpdateByIdOrDefault(int id, User entity)
    {
        return await _connection.QueryFirstOrDefaultAsync<UserModel>(
            @"UPDATE monitorpet.usuario SET Nome = @Name, Apelido = @NickName, Email = @Email, EmailConfirmado = @EmailConfirmed, 
                Login = @Login, BloqueadoAte = @LockOutEnd, ContagemErros = @AccessFailedCount 
                WHERE (IdUsuario = @Id);
            SELECT IdUsuario Id, Nome Name, Apelido NickName, Email Email, EmailConfirmado EmailConfirmed,
                Login Login, SenhaHash PasswordHash, SenhaSalt PasswordSalt, BloqueadoAte LockOutEnd, 
                ContagemErros AccessFailedCount FROM monitorpet.usuario
                WHERE IdUsuario = @Id;",
            new { Id = id, entity.Name , entity.NickName, entity.Email, entity.EmailConfirmed, entity.Login, 
            entity.LockOutEnd, entity.AccessFailedCount, },
            _transaction
        );
    }

    public async Task<UserModel?> UpdatePasswordByIdOrDefault(int id, User entity)
    {
        return await _connection.QueryFirstOrDefaultAsync<UserModel>(
            @"UPDATE monitorpet.usuario SET Nome = @Name, Apelido = @NickName, Email = @Email, EmailConfirmado = @EmailConfirmed, 
                Login = @Login, SenhaHash = @PasswordHash, SenhaSalt = @PasswordSalt, BloqueadoAte = @LockOutEnd, ContagemErros = @AccessFailedCount 
                WHERE (IdUsuario = @Id);
            SELECT IdUsuario Id, Nome Name, Apelido NickName, Email Email, EmailConfirmado EmailConfirmed,
                Login Login, SenhaHash PasswordHash, SenhaSalt PasswordSalt, BloqueadoAte LockOutEnd, 
                ContagemErros AccessFailedCount FROM monitorpet.usuario
                WHERE IdUsuario = @Id;",
            new { Id = id, entity.Name , entity.NickName, entity.Email, entity.EmailConfirmed, entity.Login, 
            entity.LockOutEnd, entity.AccessFailedCount, entity.PasswordSalt, entity.PasswordHash },
            _transaction
        );
    }
}