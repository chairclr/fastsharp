using System.Runtime.InteropServices;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe abstract class Texture : IDisposable
{
    public readonly Device Device;

    public Format Format { get; protected set; } = Format.FormatUnknown;

    protected abstract int Size { get; }

    protected abstract ComPtr<ID3D11Resource> GraphicsResource { get; }

    public bool Immutable { get; private set; }

    public bool Readable { get; private set; }

    public bool Writable { get; private set; }

    protected virtual Usage Usage => Immutable ? Usage.Immutable : ((Writable && !Readable) ? Usage.Dynamic : Usage.Default);

    protected virtual CpuAccessFlag CPUAccessFlag => (Writable ? CpuAccessFlag.Write : CpuAccessFlag.None) | (Readable ? CpuAccessFlag.Read : CpuAccessFlag.None);

    protected abstract BindFlag BindFlag { get; }

    internal ComPtr<ID3D11UnorderedAccessView> GraphicsUAV;

    internal ComPtr<ID3D11ShaderResourceView> GraphicsSRV;

    public bool Mapped { get; protected set; }

    private bool Disposed;

    public Texture(Device device, bool immutable, bool readable, bool writable)
    {
        Immutable = immutable;

        Readable = readable;

        Writable = writable;

        if (Immutable && Readable)
        {
            throw new ArgumentException("Immutable textures cannot be readable", nameof(readable));
        }

        if (Immutable && Writable)
        {
            throw new ArgumentException("Immutable textures cannot be readable", nameof(writable));
        }

        Device = device;
    }

    protected void CreateUAV(UavDimension dimension)
    {
        UnorderedAccessViewDesc unorderedAccessViewDesc = new UnorderedAccessViewDesc
        {
            Format = Format,
            ViewDimension = dimension
        };

        switch (dimension)
        {
            case UavDimension.Texture1D:
                break;
            case UavDimension.Texture2D:
                break;
            case UavDimension.Texture3D:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dimension), "Dimension must be a texture dimension");
        }

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateUnorderedAccessView(GraphicsResource, unorderedAccessViewDesc, ref GraphicsUAV));
    }

    protected void CreateSRV(D3DSrvDimension dimension)
    {
        ShaderResourceViewDesc shaderResourceViewDesc = new ShaderResourceViewDesc()
        {
            Format = Format,
            ViewDimension = dimension
        };

        switch (dimension)
        {
            case D3DSrvDimension.D3DSrvDimensionTexture1D:
                shaderResourceViewDesc.Texture1D.MipLevels = uint.MaxValue;
                break;
            case D3DSrvDimension.D3DSrvDimensionTexture2D:
                shaderResourceViewDesc.Texture2D.MipLevels = uint.MaxValue;
                break;
            case D3DSrvDimension.D3DSrvDimensionTexture3D:
                shaderResourceViewDesc.Texture3D.MipLevels = uint.MaxValue;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dimension), "Dimension must be a Texture1D, 2D, or 3D dimension");
        }

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateShaderResourceView(GraphicsResource, shaderResourceViewDesc, ref GraphicsSRV));
    }

    protected virtual void CreateView()
    {

    }

    public void CopyTo(Texture texture)
    {
        Device.GraphicsDeviceContext.CopyResource(texture.GraphicsResource, GraphicsResource);
    }

    protected Span<T> MapWrite<T>(out int rowPitch, out int depthPitch, int subresource = 0) where T : unmanaged
    {
        if (!Writable)
        {
            throw new Exception("Texture not writable");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        if (Mapped)
        {
            throw new InvalidOperationException("Cannot map an already mapped resource");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsResource, (uint)subresource, Map.WriteDiscard, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource");
        }

        Mapped = true;

        rowPitch = (int)mappedSubresource.RowPitch;
        depthPitch = (int)mappedSubresource.DepthPitch;

        return new Span<T>(mappedSubresource.PData, Size);
    }

    protected ReadOnlySpan<T> MapRead<T>(out int rowPitch, out int depthPitch, int subresource = 0) where T : unmanaged
    {
        if (!Readable)
        {
            throw new Exception("Texture not readable");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        if (Mapped)
        {
            throw new InvalidOperationException("Cannot map an already mapped resource");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsResource, (uint)subresource, Map.Read, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource", Marshal.GetExceptionForHR(hr));
        }

        Mapped = true;

        rowPitch = (int)mappedSubresource.RowPitch;
        depthPitch = (int)mappedSubresource.DepthPitch;

        return new ReadOnlySpan<T>(mappedSubresource.PData, Size);
    }

    protected Span<T> MapReadWrite<T>(out int rowPitch, out int depthPitch, int subresource = 0) where T : unmanaged
    {
        if (!Readable || !Writable)
        {
            throw new Exception("Texture not read/writable");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        if (Mapped)
        {
            throw new InvalidOperationException("Cannot map an already mapped resource");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsResource, (uint)subresource, Map.ReadWrite, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource");
        }

        Mapped = true;

        rowPitch = (int)mappedSubresource.RowPitch;
        depthPitch = (int)mappedSubresource.DepthPitch;

        return new Span<T>(mappedSubresource.PData, Size);
    }

    public void Unmap(int subresource = 0)
    {
        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        if (!Mapped)
        {
            throw new InvalidOperationException("Cannot unmap a non-mapped resource");
        }

        Mapped = false;

        Device.GraphicsDeviceContext.Unmap(GraphicsResource, (uint)subresource);
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

public abstract class Texture<T> : Texture where T : unmanaged, IComVtbl<T>, IComVtbl<ID3D11Resource>
{
    internal ComPtr<T> GraphicsTexture = default;

    protected override ComPtr<ID3D11Resource> GraphicsResource => ComPtr.Downcast<T, ID3D11Resource>(GraphicsTexture);

    private bool Disposed;

    public Texture(Device device, bool immutable, bool readable, bool writable)
        : base(device, immutable, readable, writable)
    {

    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!Disposed)
        {
            GraphicsTexture.Dispose();

            Disposed = true;
        }
    }
}
