using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class StagingTexture1D<T> : Texture<ID3D11Texture1D>
    where T : unmanaged
{
    public int Length { get; private set; } = 0;

    protected override int Size => Length;

    protected override BindFlag BindFlag => BindFlag.None;

    protected override CpuAccessFlag CPUAccessFlag => CpuAccessFlag.Read;

    protected override Usage Usage => Usage.Staging;

    public StagingTexture1D(Texture1D<T> baseTexture)
        : base(baseTexture.Device, false, true, false)
    {
        Format = baseTexture.Format;

        Length = baseTexture.Length;

        Texture1DDesc desc = new Texture1DDesc()
        {
            Width = (uint)Length,
            Usage = Usage,
            ArraySize = 1,
            BindFlags = (uint)BindFlag,
            Format = Format,
            CPUAccessFlags = (uint)CPUAccessFlag,
            MipLevels = 1
        };

        CacheDescriptionFields(desc);

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture1D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    protected override void CreateView()
    {
        throw new InvalidOperationException("Cannot create a view on a StagingTexture");
    }

    internal Texture1DDesc GetTextureDescription()
    {
        Texture1DDesc textureDesc = new Texture1DDesc();

        GraphicsTexture.GetDesc(ref textureDesc);

        return textureDesc;
    }

    private void CacheDescriptionFields(Texture1DDesc? textureDesc = null)
    {
        Texture1DDesc desc = textureDesc ?? GetTextureDescription();

        Length = (int)desc.Width;

        Format = desc.Format;
    }

    public void Read(Span<T> data, int subresource = 0)
    {
        if (data.Length != Length)
        {
            throw new ArgumentException($"Length of {nameof(data)} must be equal to length of texture", nameof(data));
        }

        ReadOnlySpan<T> span = MapRead<T>(out _, out _, subresource);

        span.CopyTo(data);

        Unmap(subresource);
    }

    public T[] Read(int subresource = 0)
    {
        ReadOnlySpan<T> span = MapRead<T>(out _, out _, subresource);

        T[] values = new T[Length];

        span.CopyTo(values);

        Unmap(subresource);

        return values;
    }

    public ReadOnlySpan<T> MapRead(int subresource = 0)
    {
        return MapRead<T>(out _, out _, subresource);
    }
}
