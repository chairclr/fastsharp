using Silk.NET.Core.Native;

namespace FastSharp;

internal static class ComPtrExtensions
{
    public static unsafe bool IsNull<T>(this ComPtr<T> ptr)
        where T : unmanaged, IComVtbl<T>
    {
        return ptr.Handle == null;
    }
}
