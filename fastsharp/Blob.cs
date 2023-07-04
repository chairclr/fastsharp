using System.Runtime.CompilerServices;
using Silk.NET.Core.Native;

namespace FastSharp;

public class Blob : IDisposable
{
    internal ComPtr<ID3D10Blob> GraphicsBlob = default;

    private bool Disposed;

    public bool IsNull => Unsafe.IsNullRef(ref GraphicsBlob.Get());

    public nuint Size => GraphicsBlob.GetBufferSize();

    public unsafe string? AsString()
    {
        return SilkMarshal.PtrToString((nint)GraphicsBlob.GetBufferPointer());
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {

            Disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
