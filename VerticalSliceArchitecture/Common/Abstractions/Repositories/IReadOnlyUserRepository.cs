using VerticalSliceArchitecture.Common.Contracts.Users;
using VerticalSliceArchitecture.Domain.Users;

namespace VerticalSliceArchitecture.Common.Abstractions.Repositories;
public interface IReadOnlyUserRepository
{
    Task<bool> ExistsEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<UserDto>> GetAllDtosAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetDtoByIdAsync(UserId id, CancellationToken cancellationToken = default);
}
