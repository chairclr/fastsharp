using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class Texture2D : Texture<ID3D11Texture2D>
{
    public int Width { get; private set; } = 0;

    public int Height { get; private set; } = 0;

    protected override int Size => Width * Height;

    internal Texture2D(Device device, Texture2DDesc desc)
        : base(device)
    {
        CacheDescriptionFields(desc);

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    internal Texture2D(Device device, Texture2DDesc desc, Span<byte> initialData)
        : base(device)
    {
        CacheDescriptionFields(desc);

        SubresourceData subresourceData = new SubresourceData()
        {
            PSysMem = Unsafe.AsPointer(ref initialData.DangerousGetReference())
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, subresourceData, ref GraphicsTexture));
    }

    public Texture2D(Device device, int width, int height, Format format = Format.FormatR8G8B8A8Unorm, Usage usage = Usage.Default, BindFlag bindFlag = BindFlag.UnorderedAccess, CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None)
        : base(device)
    {
        Texture2DDesc desc = new Texture2DDesc()
        {
            Width = (uint)width,
            Height = (uint)height,
            Usage = usage,
            SampleDesc = new SampleDesc(1, 0),
            ArraySize = 1,
            BindFlags = (uint)bindFlag,
            Format = format,
            CPUAccessFlags = (uint)cpuAccessFlag,
            MipLevels = 1
        };

        CacheDescriptionFields(desc);

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    public Texture2D(Device device, int width, int height, Span<byte> initialData, Format format = Format.FormatR8G8B8A8Unorm, Usage usage = Usage.Default, BindFlag bindFlag = BindFlag.UnorderedAccess, CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None)
        : base(device)
    {
        Texture2DDesc desc = new Texture2DDesc()
        {
            Width = (uint)width,
            Height = (uint)height,
            Usage = usage,
            SampleDesc = new SampleDesc(1, 0),
            ArraySize = 1,
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
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, subresourceData, ref GraphicsTexture));
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

    internal Texture2D CreateStagingTexture()
    {
        Texture2DDesc desc = GetTextureDescription();

        desc = desc with
        {
            Usage = Usage.Staging,
            CPUAccessFlags = (uint)CpuAccessFlag.Read,
            MipLevels = 1,
            BindFlags = (uint)BindFlag.None,
            SampleDesc = new SampleDesc(1, 0),
        };


        return new Texture2D(Device, desc);
    }
}
