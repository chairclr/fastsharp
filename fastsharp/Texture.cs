using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;

namespace FastSharp;

public abstract class Texture<T> : IDisposable where T : unmanaged, IComVtbl<T>
{
    internal ComPtr<T> GraphicsTexture = default;

    private bool Disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            GraphicsTexture.Dispose();

            Disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
