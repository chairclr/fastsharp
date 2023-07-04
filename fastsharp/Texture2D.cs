using System.Runtime.CompilerServices;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class Texture2D : Texture<ID3D11Texture2D>, IDisposable, IMappableResource
{
    public readonly Device Device;

    public readonly int Width = 0;

    public readonly int Height = 0;

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

    public Span<T> MapWrite<T>(int subresource = 0) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<T> MapRead<T>(int subresource = 0) where T : unmanaged
    {
        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsTexture, (uint)subresource, Map.Read, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource");
        }

        return new ReadOnlySpan<T>(mappedSubresource.PData, Width * Height);
    }

    public void Unmap(int subresource = 0)
    {
        Device.GraphicsDeviceContext.Unmap(GraphicsTexture, (uint)subresource);
    }
}
