using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonitorPet.Application.Model.User;
using MonitorPet.Application.Repositories;
using MonitorPet.Application.Tests.InMemoryDb;
using MonitorPet.Application.Tests.ModelDb;
using MonitorPet.Application.Tests.UoW;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Tests.Repositories;

internal class UserRepository : RepositoryBase, IUserRepository
{
    public UserRepository(IMemorySession memorySession, IMapper mapper) 
        : base(memorySession, mapper)
    {
    }

    public async Task<UserModel> Create(User entity)
    {
        var userDbModel = _mapper.Map<UserDbModel>(entity);

        await _context.Users.AddAsync(userDbModel);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserModel>(userDbModel);
    }

    public async Task<UserModel?> DeleteByIdOrDefault(int id)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userDb is null)
            return null;

        _context.Users.Remove(userDb);

        await _context.SaveChangesAsync();

        return _mapper.Map<UserModel>(userDb);
    }

    public async Task<IEnumerable<UserModel>> GetAll()
        => (await _context.Users.AsNoTracking().ToListAsync())
            .Select(userDb => _mapper.Map<UserModel>(userDb));

    public async Task<UserModel?> GetByEmailOrDefault(string email)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (userDb is null)
            return null;

        return _mapper.Map<UserModel>(userDb);
    }

    public async Task<UserModel?> GetByIdOrDefault(int id)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userDb is null)
            return null;

        return _mapper.Map<UserModel>(userDb);
    }

    public async Task<UserModel?> UpdateAccessAccountFailed(int id, User entity)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userDb is null)
            return null;

        var userToUpdate = _mapper.Map<UserDbModel>(entity);
        userDb.AccessFailedCount = userToUpdate.AccessFailedCount;
        userDb.LockOutEnd = userToUpdate.LockOutEnd;

        _context.Users.Update(userDb);

        await _context.SaveChangesAsync();

        return _mapper.Map<UserModel>(
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
        );
    }

    public async Task<UserModel?> UpdateByIdOrDefault(int id, User entity)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userDb is null)
            return null;

        var userToUpdate = _mapper.Map<UserDbModel>(entity);
        userDb.AccessFailedCount = userToUpdate.AccessFailedCount;
        userDb.LockOutEnd = userToUpdate.LockOutEnd;
        userDb.Email = userToUpdate.Email;
        userDb.EmailConfirmed = userToUpdate.EmailConfirmed;
        userDb.LockOutEnd = userToUpdate.LockOutEnd;
        userDb.Name = userToUpdate.Name;
        userDb.NickName = userToUpdate.NickName;

        _context.Users.Update(userDb);

        await _context.SaveChangesAsync();

        return _mapper.Map<UserModel>(
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
        );
    }

    public async Task<UserModel?> UpdatePasswordByIdOrDefault(int id, User entity)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userDb is null)
            return null;

        var userToUpdate = _mapper.Map<UserDbModel>(entity);
        userDb.PasswordSalt = userToUpdate.PasswordSalt;
        userDb.PasswordHash = userToUpdate.PasswordHash;
        userDb.AccessFailedCount = userToUpdate.AccessFailedCount;
        userDb.LockOutEnd = userToUpdate.LockOutEnd;
        userDb.Email = userToUpdate.Email;
        userDb.EmailConfirmed = userToUpdate.EmailConfirmed;
        userDb.LockOutEnd = userToUpdate.LockOutEnd;
        userDb.Name = userToUpdate.Name;
        userDb.NickName = userToUpdate.NickName;

        _context.Users.Update(userDb);

        await _context.SaveChangesAsync();

        return _mapper.Map<UserModel>(
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
        );
    }
}