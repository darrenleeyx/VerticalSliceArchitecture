namespace VerticalSliceArchitecture.Common.Abstractions;

public interface IUnitOfWork
{
    Task<int> CommitChangesAsync(CancellationToken cancellationToken = default);
}