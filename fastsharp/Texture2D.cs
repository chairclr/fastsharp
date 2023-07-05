using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class Texture2D : Texture<ID3D11Texture2D>
{
    public int Width { get; private set; } = 0;

    public int Height { get; private set; } = 0;

    protected override int Size => Width * Height;

    internal Texture2D(Device device)
        : base(device)
    {

    }

    public Texture2D(Device device, int width, int height)
        : base(device)
    {
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
            Format = Format.FormatR8G8B8A8Unorm,
        };

        Format = desc.Format;

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    public Texture2D(Device device, int width, int height, Format format)
        : base(device)
    {
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
            Format = format
        };

        Format = desc.Format;

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, (SubresourceData*)null, ref GraphicsTexture));
    }

    internal Texture2DDesc GetTextureDescription()
    {
        Texture2DDesc textureDesc = new Texture2DDesc();

        GraphicsTexture.GetDesc(ref textureDesc);

        return textureDesc;
    }

    internal Texture2D CreateStagingTexture()
    {
        Texture2D stagingTexture = new Texture2D(Device);

        Texture2DDesc desc = GetTextureDescription();

        stagingTexture.Width = Width;

        stagingTexture.Height = Height;

        stagingTexture.Format = Format;

        desc = desc with
        {
            Usage = Usage.Staging,
            CPUAccessFlags = (uint)CpuAccessFlag.Read,
            MipLevels = 0,
            BindFlags = (uint)BindFlag.None,
            SampleDesc = new SampleDesc(1, 0)
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateTexture2D(desc, (SubresourceData*)null, ref stagingTexture.GraphicsTexture));

        return stagingTexture;
    }
}
