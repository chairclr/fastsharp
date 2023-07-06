using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class Texture1D : Texture<ID3D11Texture1D>
{
    public int Length { get; private set; } = 0;

    protected override int Size => Length;

    internal Texture1D(Device device, Texture1DDesc desc)
        : base(device)
    {
        CacheDescriptionFields(desc);

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture1D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    internal Texture1D(Device device, Texture1DDesc desc, ReadOnlySpan<byte> initialData)
        : base(device)
    {
        CacheDescriptionFields(desc);

        SubresourceData subresourceData = new SubresourceData()
        {
            PSysMem = Unsafe.AsPointer(ref initialData.DangerousGetReference())
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture1D(desc, subresourceData, ref GraphicsTexture));
    }

    public Texture1D(Device device, int length, Format format = Format.FormatR8G8B8A8Unorm, Usage usage = Usage.Default, BindFlag bindFlag = BindFlag.UnorderedAccess, CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None)
        : base(device)
    {
        Texture1DDesc desc = new Texture1DDesc()
        {
            Width = (uint)length,
            Usage = usage,
            ArraySize = 1,
            BindFlags = (uint)bindFlag,
            Format = format,
            CPUAccessFlags = (uint)cpuAccessFlag,
            MipLevels = 1
        };

        CacheDescriptionFields(desc);

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture1D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    public Texture1D(Device device, int length, ReadOnlySpan<byte> initialData, Format format = Format.FormatR8G8B8A8Unorm, Usage usage = Usage.Default, BindFlag bindFlag = BindFlag.UnorderedAccess, CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None)
        : base(device)
    {
        Texture1DDesc desc = new Texture1DDesc()
        {
            Width = (uint)length,
            Usage = usage,
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
            SysMemPitch = (uint)Length,
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture1D(desc, subresourceData, ref GraphicsTexture));
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

    internal Texture1D CreateStagingTexture()
    {
        Texture1DDesc desc = GetTextureDescription();

        desc = desc with
        {
            Usage = Usage.Staging,
            CPUAccessFlags = (uint)CpuAccessFlag.Read,
            MipLevels = 1,
            BindFlags = (uint)BindFlag.None
        };


        return new Texture1D(Device, desc);
    }
}
