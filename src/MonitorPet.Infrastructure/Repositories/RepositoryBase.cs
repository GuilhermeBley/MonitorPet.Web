using System.Data;
using MonitorPet.Infrastructure.UoW;

namespace MonitorPet.Infrastructure.Repositories;

public class RepositoryBase
{
    private MySqlSession _mysqlSession;
    public IDbConnection _connection => _mysqlSession.Connection;
    public IDbTransaction? _transaction => _mysqlSession.Transaction;

    public RepositoryBase(MySqlSession mysqlSession)
    {
        _mysqlSession = mysqlSession;
    }
}
