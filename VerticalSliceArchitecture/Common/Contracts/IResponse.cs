namespace VerticalSliceArchitecture.Common.Contracts;

public interface IResponse<T>
{
    T Value { get; }
}
