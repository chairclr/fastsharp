using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe abstract class Texture : IMappableResource, IDisposable
{
    public readonly Device Device;

    public Format Format { get; protected set; } = Format.FormatUnknown;

    protected abstract int Size { get; }

    protected abstract ComPtr<ID3D11Resource> GraphicsResource { get; }

    protected ComPtr<ID3D11UnorderedAccessView> GraphicsUAVCache;
    internal ref ComPtr<ID3D11UnorderedAccessView> GraphicsUAV
    {
        get
        {
            if (GraphicsUAVCache.IsNull())
            {
                GraphicsUAVCache = CreateUAV();
            }

            return ref GraphicsUAVCache;
        }
    }

    protected ComPtr<ID3D11ShaderResourceView> GraphicsSRVCache;
    internal ref ComPtr<ID3D11ShaderResourceView> GraphicsSRV
    {
        get
        {
            if (GraphicsSRVCache.IsNull())
            {
                GraphicsSRVCache = CreateSRV();
            }

            return ref GraphicsSRVCache;
        }
    }

    private bool Disposed;

    public Texture(Device device)
    {
        Device = device;
    }

    private ComPtr<ID3D11UnorderedAccessView> CreateUAV()
    {
        ComPtr<ID3D11UnorderedAccessView> accessView = default;

        UnorderedAccessViewDesc unorderedAccessViewDesc = new UnorderedAccessViewDesc()
        {
            Format = Format
        };

        if (this is Texture2D)
        {
            unorderedAccessViewDesc.ViewDimension = UavDimension.Texture2D;
        }

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateUnorderedAccessView(GraphicsResource, unorderedAccessViewDesc, ref accessView));

        return accessView;
    }

    private ComPtr<ID3D11ShaderResourceView> CreateSRV()
    {
        ComPtr<ID3D11ShaderResourceView> resourceView = default;

        ShaderResourceViewDesc shaderResourceViewDesc = new ShaderResourceViewDesc()
        {
            Format = Format,
        };

        if (this is Texture2D)
        {
            shaderResourceViewDesc.ViewDimension = D3DSrvDimension.D3D101SrvDimensionTexture2D;
            shaderResourceViewDesc.Texture2D.MipLevels = uint.MaxValue;
        }

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateShaderResourceView(GraphicsResource, shaderResourceViewDesc, ref resourceView));

        return resourceView;
    }

    public void CopyTo(Texture texture)
    {
        Device.GraphicsDeviceContext.CopyResource(texture.GraphicsResource, GraphicsResource);
    }

    public Span<T> MapWrite<T>(int subresource = 0) where T : unmanaged
    {
        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsResource, (uint)subresource, Map.Write, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource");
        }

        return new Span<T>(mappedSubresource.PData, Size);
    }

    public ReadOnlySpan<T> MapRead<T>(int subresource = 0) where T : unmanaged
    {
        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsResource, (uint)subresource, Map.Read, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource");
        }

        return new ReadOnlySpan<T>(mappedSubresource.PData, Size);
    }

    public Span<T> MapReadWrite<T>(int subresource = 0) where T : unmanaged
    {
        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsResource, (uint)subresource, Map.ReadWrite, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource");
        }

        return new Span<T>(mappedSubresource.PData, Size);
    }

    public void Unmap(int subresource = 0)
    {
        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        Device.GraphicsDeviceContext.Unmap(GraphicsResource, (uint)subresource);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            GraphicsUAVCache.Dispose();

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

    public Texture(Device device)
        : base(device)
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
