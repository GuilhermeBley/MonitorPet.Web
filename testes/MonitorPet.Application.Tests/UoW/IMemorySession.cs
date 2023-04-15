namespace MonitorPet.Application.Tests.UoW;

internal interface IMemorySession
{
    InMemoryDb.AppDbContext Context { get; }
}