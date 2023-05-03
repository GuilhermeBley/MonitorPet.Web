using Dapper;
using MonitorPet.Application.Model.PesoHistorico;
using MonitorPet.Application.Repositories;
using MonitorPet.Infrastructure.UoW;

namespace MonitorPet.Infrastructure.Repositories;

internal class WeightHistoryRepository : RepositoryBase, IWeightHistoryRepository
{
    public WeightHistoryRepository(MySqlSession mysqlSession) : base(mysqlSession)
    {
    }

    public IAsyncEnumerable<WeightHistoryModel> GetByDosador(Guid idDosador)
    {
        return GetByDosadorAndInterval(idDosador, DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
    }

    public async IAsyncEnumerable<WeightHistoryModel> GetByDosadorAndInterval(Guid idDosador, DateTimeOffset start, DateTimeOffset? end = null)
    {
        if (end is null)
            end = DateTimeOffset.MaxValue;

        const int MAX_TAKE = 100;
        const string QUERY = @"
SELECT 
    Id Id,
    PesoGr Weight,
    DateAt RegisteredAt
FROM monitorpet.historicopeso
WHERE IdDosador = @IdDosador
AND DateAt >= @Start
AND DateAt < @End
LIMIT @Skip, @Take;";

        for (int pageIndex = 0; ; pageIndex++)
        {
            var items = await _connection.QueryAsync<WeightHistoryModel>(
                QUERY,
                new { 
                    IdDosador = idDosador,
                    Start = start.DateTime,
                    End = end.Value.DateTime,
                    Skip = pageIndex* MAX_TAKE,
                    Take = MAX_TAKE
                },
                _transaction
            );

            if (!items.Any())
                yield break;

            foreach (var item in items)
                yield return item;
        }
    }

    public async Task<WeightHistoryModel?> GetByIdOrDefault(long id)
    {
        const string QUERY = @"
SELECT 
    Id Id,
    PesoGr Weight,
    DateAt RegisteredAt
FROM monitorpet.historicopeso
WHERE Id = @Id;";

        return await _connection.QueryFirstOrDefaultAsync<WeightHistoryModel>(
            QUERY,
            new { Id = id },
            _transaction
        );
    }
}
