using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Common.Abstractions.Repositories;
using VerticalSliceArchitecture.Common.Contracts.Users;
using VerticalSliceArchitecture.Domain.Users;

namespace VerticalSliceArchitecture.Infrastructure.Users;

public class UserRepository : IReadOnlyUserRepository, IReadWriteUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<List<UserDto>> GetAllDtosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Select(u => u.ToDto())
            .ToListAsync(cancellationToken);
    }

    public Task<UserDto?> GetDtoByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => u.ToDto())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<User?> GetByIdAsync(UserId id, bool track = true, CancellationToken cancellationToken = default)
    {
        var dbSet = _context.Users;

        var query = track ? dbSet.AsTracking() : dbSet.AsNoTracking();

        return query
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
    }

    public void Remove(User user)
    {
        _context.Users.Remove(user);
    }
}

public static class UserMapping
{
    public static UserDto ToDto(this User user)
        => new(user.Name, user.Email);
}
