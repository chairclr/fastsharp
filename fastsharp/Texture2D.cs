using System.Runtime.CompilerServices;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class Texture2D : IDisposable, IMappableResource
{
    public readonly Device Device;

    public readonly int Width;

    public readonly int Height;

    internal ComPtr<ID3D11Texture2D> GraphicsTexture;

    private bool Disposed;

    public Texture2D(Device device, int width, int height)
    {
        Device = device;

        Width = width;

        Height = height;

        Texture2DDesc desc = new Texture2DDesc()
        {
            Width = (uint)width,
            Height = (uint)height,
            Usage = Usage.Default,
            SampleDesc = new SampleDesc(1, 0),
            ArraySize = 1,
            BindFlags = (uint)BindFlag.UnorderedAccess,
            Format = Format.FormatR8G8B8A8Unorm
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    public Span<T> MapWrite<T>(Span<T> span) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<T> MapRead<T>(Span<T> span) where T : unmanaged
    {
        throw new NotImplementedException();
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
