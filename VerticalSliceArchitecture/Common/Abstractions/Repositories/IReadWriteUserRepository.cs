using VerticalSliceArchitecture.Domain.Users;

namespace VerticalSliceArchitecture.Common.Abstractions.Repositories;

public interface IReadWriteUserRepository : IReadOnlyUserRepository
{
    Task<User?> GetByIdAsync(UserId id, bool track = true, CancellationToken cancellationToken = default);
    void Add(User user);
    void Remove(User user);
}