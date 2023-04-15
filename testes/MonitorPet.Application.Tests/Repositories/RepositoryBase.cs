using AutoMapper;
using MonitorPet.Application.Tests.InMemoryDb;
using MonitorPet.Application.Tests.UoW;

namespace MonitorPet.Application.Tests.Repositories;

internal class RepositoryBase
{
    private readonly IMemorySession _memorySession;
    protected AppDbContext _context => _memorySession.Context;
    protected readonly IMapper _mapper;

    public RepositoryBase(
        IMemorySession memorySession,
        IMapper mapper)
    {
        _memorySession = memorySession;
        _mapper = mapper;
    }
}