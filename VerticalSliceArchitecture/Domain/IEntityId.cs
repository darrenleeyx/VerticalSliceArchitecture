namespace VerticalSliceArchitecture.Domain;

public interface IEntityId<T>
{
    T Value { get; }
}
