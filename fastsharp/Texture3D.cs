using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class Texture3D : Texture<ID3D11Texture3D>
{
    public int Width { get; private set; } = 0;

    public int Height { get; private set; } = 0;

    public int Depth { get; private set; } = 0;

    protected override int Size => Width * Height * Depth;

    internal Texture3D(Device device, Texture3DDesc desc)
        : base(device)
    {
        CacheDescriptionFields(desc);

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture3D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    internal Texture3D(Device device, Texture3DDesc desc, ReadOnlySpan<byte> initialData)
        : base(device)
    {
        CacheDescriptionFields(desc);

        SubresourceData subresourceData = new SubresourceData()
        {
            PSysMem = Unsafe.AsPointer(ref initialData.DangerousGetReference()),
            SysMemPitch = (uint)Width,
            SysMemSlicePitch = (uint)Height,
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture3D(desc, subresourceData, ref GraphicsTexture));
    }

    public Texture3D(Device device, int width, int height, int depth, Format format = Format.FormatR8G8B8A8Unorm, Usage usage = Usage.Default, BindFlag bindFlag = BindFlag.UnorderedAccess, CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None)
        : base(device)
    {
        Texture3DDesc desc = new Texture3DDesc()
        {
            Width = (uint)width,
            Height = (uint)height,
            Depth = (uint)depth,
            Usage = usage,
            BindFlags = (uint)bindFlag,
            Format = format,
            CPUAccessFlags = (uint)cpuAccessFlag,
            MipLevels = 1
        };

        CacheDescriptionFields(desc);

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture3D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    public Texture3D(Device device, int width, int height, int depth, ReadOnlySpan<byte> initialData, Format format = Format.FormatR8G8B8A8Unorm, Usage usage = Usage.Default, BindFlag bindFlag = BindFlag.UnorderedAccess, CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None)
        : base(device)
    {
        Texture3DDesc desc = new Texture3DDesc()
        {
            Width = (uint)width,
            Height = (uint)height,
            Depth = (uint)depth,
            Usage = usage,
            BindFlags = (uint)bindFlag,
            Format = format,
            CPUAccessFlags = (uint)cpuAccessFlag,
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

    internal Texture3D CreateStagingTexture()
    {
        Texture3DDesc desc = GetTextureDescription();

        desc = desc with
        {
            Usage = Usage.Staging,
            CPUAccessFlags = (uint)CpuAccessFlag.Read,
            MipLevels = 1,
            BindFlags = (uint)BindFlag.None,
        };

        return new Texture3D(Device, desc);
    }
}
