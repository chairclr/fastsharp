using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class Texture2D<T> : Texture<ID3D11Texture2D>
    where T : unmanaged
{
    public int Width { get; private set; } = 0;

    public int Height { get; private set; } = 0;

    protected override int Size => Width * Height;

    protected override BindFlag BindFlag => BindFlag.ShaderResource;

    public Texture2D(Device device, Format format, int width, int height, bool immutable = false, bool writable = false)
        : base(device, immutable, false, writable)
    {
        if (immutable)
        {
            throw new ArgumentException("Cannot create an immutable texture without initial data");
        }

        if (format == Format.FormatUnknown)
        {
            throw new ArgumentOutOfRangeException(nameof(format), "Must provide a known format");
        }

        if (width < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), $"Width must be greater than zero");
        }

        if (height < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), $"Height must be greater than zero");
        }

        Format = format;

        Width = width;

        Height = height;

        Texture2DDesc desc = new Texture2DDesc()
        {
            Width = (uint)Width,
            Height = (uint)Height,
            Usage = Usage,
            ArraySize = 1,
            BindFlags = (uint)BindFlag,
            Format = Format,
            CPUAccessFlags = (uint)CPUAccessFlag,
            MipLevels = 1,
            SampleDesc = new SampleDesc(1)
        };

        CacheDescriptionFields(desc);

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, (SubresourceData*)null, ref GraphicsTexture));

        CreateView();
    }

    public Texture2D(Device device, Format format, int width, int height, ReadOnlySpan<T> initialData, bool immutable = false, bool writable = false)
        : base(device, immutable, false, writable)
    {
        if (format == Format.FormatUnknown)
        {
            throw new ArgumentOutOfRangeException(nameof(format), "Must provide a known format");
        }

        if (width < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), $"Width must be greater than zero");
        }

        if (height < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), $"Height must be greater than zero");
        }

        if (initialData.Length != width * height)
        {
            throw new ArgumentException($"Length of {nameof(initialData)} must be equal to {nameof(width)} * {nameof(height)}");
        }

        Format = format;

        Width = width;

        Height = height;

        Texture2DDesc desc = new Texture2DDesc()
        {
            Width = (uint)Width,
            Height = (uint)Height,
            Usage = Usage,
            ArraySize = 1,
            BindFlags = (uint)BindFlag,
            Format = Format,
            CPUAccessFlags = (uint)CPUAccessFlag,
            MipLevels = 1,
            SampleDesc = new SampleDesc(1)
        };

        CacheDescriptionFields(desc);

        SubresourceData subresourceData = new SubresourceData()
        {
            PSysMem = Unsafe.AsPointer(ref initialData.DangerousGetReference()),
            SysMemPitch = (uint)Width * (uint)Unsafe.SizeOf<T>()
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, subresourceData, ref GraphicsTexture));

        CreateView();
    }

    protected override void CreateView()
    {
        CreateSRV(D3DSrvDimension.D3D101SrvDimensionTexture2D);
    }

    internal Texture2DDesc GetTextureDescription()
    {
        Texture2DDesc textureDesc = new Texture2DDesc();

        GraphicsTexture.GetDesc(ref textureDesc);

        return textureDesc;
    }

    private void CacheDescriptionFields(Texture2DDesc? textureDesc = null)
    {
        Texture2DDesc desc = textureDesc ?? GetTextureDescription();

        Width = (int)desc.Width;

        Height = (int)desc.Height;

        Format = desc.Format;
    }

    public void Write(ReadOnlySpan2D<T> data, int subresource = 0)
    {
        Span<T> span = MapWrite<T>(out int rowPitch, out _, subresource);
        Span2D<T> span2d = new Span2D<T>(Unsafe.AsPointer(ref span.DangerousGetReference()), Height, Width, rowPitch / Unsafe.SizeOf<T>() - Width);

        data.CopyTo(span2d);

        Unmap();
    }

    public void Write(T[,] data, int subresource = 0)
    {
        Span<T> span = MapWrite<T>(out int rowPitch, out _, subresource);
        Span2D<T> span2d = new Span2D<T>(Unsafe.AsPointer(ref span.DangerousGetReference()), Height, Width, rowPitch / Unsafe.SizeOf<T>() - Width);

        data.AsSpan2D().CopyTo(span2d);

        Unmap();
    }

    public Span2D<T> MapWrite(int subresource = 0)
    {
        Span<T> span = MapWrite<T>(out int rowPitch, out _, subresource);

        return new Span2D<T>(Unsafe.AsPointer(ref span.DangerousGetReference()), Height, Width, rowPitch / Unsafe.SizeOf<T>() - Width);
    }
}
