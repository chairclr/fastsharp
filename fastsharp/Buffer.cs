using System.Runtime.CompilerServices;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;

namespace FastSharp;

public abstract class Buffer : IDisposable
{
    public readonly Device Device;

    internal ComPtr<ID3D11Buffer> GraphicsBuffer = default;

    public virtual uint Length { get; protected set; } = 0;

    private bool Disposed;

    public Buffer(Device device)
    {
        Device = device;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            GraphicsBuffer.Dispose();

            Disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

public abstract class Buffer<T> : Buffer
    where T : unmanaged
{
    public uint Stride => (uint)Unsafe.SizeOf<T>();

    public uint Size => Stride * Length;

    protected Buffer(Device device)
        : base(device)
    {

    }
}
