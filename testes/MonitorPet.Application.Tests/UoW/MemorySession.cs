using MonitorPet.Application.Tests.InMemoryDb;
using MonitorPet.Application.UoW;

namespace MonitorPet.Application.Tests.UoW;

internal class MemorySession : IUnitOfWork, IMemorySession
{
    private readonly AppDbContext _context;
    private bool _hasConnection { get; set; }
    private bool _hasTransaction { get; set; }
    public Guid IdSession { get; } = Guid.NewGuid();
    public AppDbContext Context => _hasConnection ? _context : throw new InvalidOperationException("Connection not oppened.");

    public MemorySession(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IUnitOfWork> BeginTransactionAsync()
    {
        _hasConnection = true;
        _hasTransaction = true;
        await Task.CompletedTask;
        return this;
    }

    public void Dispose()
    {
        _hasConnection = false;
        _hasTransaction = false;
    }

    public async Task<IUnitOfWork> OpenConnectionAsync()
    {
        _hasConnection = true;
        await Task.CompletedTask;
        return this;
    }

    public async Task RollBackAsync()
    {
        ThrowIfDontContainTransaction();
        _hasTransaction = false;
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        ThrowIfDontContainTransaction();
        _hasTransaction = false;
        await Task.CompletedTask;
    }

    private void ThrowIfDontContainTransaction()
    {
        if (!_hasConnection|| !_hasTransaction)
            throw new InvalidOperationException("Transaction not oppened.");
    }
}