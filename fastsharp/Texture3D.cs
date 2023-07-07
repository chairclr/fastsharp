using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class Texture3D<T> : Texture<ID3D11Texture3D>
    where T : unmanaged
{
    public int Width { get; private set; } = 0;

    public int Height { get; private set; } = 0;

    public int Depth { get; private set; } = 0;

    protected override int Size => Width * Height * Depth;

    protected override BindFlag BindFlag => BindFlag.ShaderResource;

    public Texture3D(Device device, Format format, int width, int height, int depth, bool immutable = false, bool writable = false)
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

        if (depth < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(depth), $"Depth must be greater than zero");
        }

        Format = format;

        Width = width;

        Height = height;

        Depth = depth;

        Texture3DDesc desc = new Texture3DDesc()
        {
            Width = (uint)Width,
            Height = (uint)Height,
            Depth = (uint)Depth,
            Usage = Usage,
            BindFlags = (uint)BindFlag,
            Format = Format,
            CPUAccessFlags = (uint)CPUAccessFlag,
            MipLevels = 1
        };

        CacheDescriptionFields(desc);

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture3D(desc, (SubresourceData*)null, ref GraphicsTexture));

        CreateView();
    }

    public Texture3D(Device device, Format format, int width, int height, int depth, ReadOnlySpan<T> initialData, bool immutable = false, bool writable = false)
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
            throw new ArgumentOutOfRangeException(nameof(height), $"Height must be greater than zero");
        }

        if (depth < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(depth), $"Depth must be greater than zero");
        }

        if (initialData.Length != width * height * depth)
        {
            throw new ArgumentException($"Length of {nameof(initialData)} must be equal to the size of the texture (Width * Height)", nameof(initialData));
        }

        Format = format;

        Width = width;

        Height = height;

        Depth = depth;

        Texture3DDesc desc = new Texture3DDesc()
        {
            Width = (uint)Width,
            Height = (uint)Height,
            Depth = (uint)Depth,
            Usage = Usage,
            BindFlags = (uint)BindFlag,
            Format = Format,
            CPUAccessFlags = (uint)CPUAccessFlag,
            MipLevels = 1
        };

        CacheDescriptionFields(desc);

        SubresourceData subresourceData = new SubresourceData()
        {
            PSysMem = Unsafe.AsPointer(ref initialData.DangerousGetReference()),
            SysMemPitch = (uint)Width,
            SysMemSlicePitch = (uint)Height,
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture3D(desc, subresourceData, ref GraphicsTexture));

        CreateView();
    }

    protected override void CreateView()
    {
        CreateSRV(D3DSrvDimension.D3D101SrvDimensionTexture3D);
    }

    internal Texture3DDesc GetTextureDescription()
    {
        Texture3DDesc textureDesc = new Texture3DDesc();

        GraphicsTexture.GetDesc(ref textureDesc);

        return textureDesc;
    }

    private void CacheDescriptionFields(Texture3DDesc? textureDesc = null)
    {
        Texture3DDesc desc = textureDesc ?? GetTextureDescription();

        Width = (int)desc.Width;

        Height = (int)desc.Height;

        Depth = (int)desc.Depth;

        Format = desc.Format;
    }
}
