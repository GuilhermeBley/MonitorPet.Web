using Dapper;
using MonitorPet.Application.Model.Email;
using MonitorPet.Application.Repositories;
using MonitorPet.Infrastructure.UoW;

namespace MonitorPet.Infrastructure.Repositories;

internal class EmailTypeRepository : RepositoryBase, IEmailTypeRepository
{
    public EmailTypeRepository(MySqlSession mysqlSession) : base(mysqlSession)
    {
    }

    public async Task<IEnumerable<EmailTypeModel>> GetAll()
        => await _connection.QueryAsync<EmailTypeModel>(
            "SELECT Id Id, TipoEnvio Type, Descricao Description FROM monitorpet.tipoemail;",
            transaction: _transaction
        );
}
