using System;

namespace Project.Code
{
    public interface IReadOnlyValue<T>
    {
        T Value { get; }
        IDisposable Subscribe(Action<T> cb, bool invokeImmediately = true);
    }
}