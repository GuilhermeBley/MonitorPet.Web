using Dapper;
using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Repositories;
using MonitorPet.Core.Entity;
using MonitorPet.Infrastructure.UoW;
using System.Text;
using static Dapper.SqlMapper;

namespace MonitorPet.Infrastructure.Repositories;

public class DosadorRepository : RepositoryBase, IDosadorRepository
{
    public DosadorRepository(MySqlSession mysqlSession) : base(mysqlSession)
    {
    }

    public Task<DosadorModel> Create(Dosador entity)
    {
        throw new NotImplementedException();
    }

    public Task<DosadorModel?> DeleteByIdOrDefault(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DosadorModel>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<DosadorModel?> GetByIdOrDefault(Guid id)
    {
        return await _connection.QueryFirstOrDefaultAsync<DosadorModel>(
            @"SELECT IdDosador IdDosador, Nome Nome, ImgUrl, UltimaAtualizacao LastRefresh, UltimaLiberacao LastRelease FROM monitorpet.dosador
                WHERE IdDosador = @IdDosador;",
            new { IdDosador = id },
            _transaction
        );
    }

    public async Task<DosadorModel?> UpdateByIdOrDefault(Guid id, Dosador entity)
    {
        return await _connection.QueryFirstOrDefaultAsync<DosadorModel>(
            @"UPDATE monitorpet.dosador SET Nome = @Nome, ImgUrl = @ImgUrl WHERE (IdDosador = @IdDosador);
            SELECT IdDosador IdDosador, Nome Nome, ImgUrl, UltimaAtualizacao LastRefresh, UltimaLiberacao LastRelease FROM monitorpet.dosador
                WHERE IdDosador = @IdDosador;",
            new { IdDosador = id, Nome = entity.Nome, ImgUrl = entity.ImgUrl },
            _transaction
        );
    }

    public async Task<DosadorModel?> UpdateLastRelease(Guid IdDosador, DateTime lastRelease)
    {
        return await _connection.QueryFirstOrDefaultAsync<DosadorModel>(
            @"UPDATE monitorpet.dosador SET UltimaLiberacao = @LastRelease WHERE (IdDosador = @IdDosador);
            SELECT IdDosador IdDosador, Nome Nome, ImgUrl, UltimaAtualizacao LastRefresh, UltimaLiberacao LastRelease FROM monitorpet.dosador
                WHERE IdDosador = @IdDosador;"
        ,
        new { IdDosador = IdDosador, LastRelease = lastRelease },
            _transaction
        );
    }
}