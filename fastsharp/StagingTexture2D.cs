using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class StagingTexture2D<T> : Texture<ID3D11Texture2D>
    where T : unmanaged
{
    public int Width { get; private set; } = 0;

    public int Height { get; private set; } = 0;

    protected override int Size => Width * Height;

    protected override BindFlag BindFlag => BindFlag.None;

    protected override CpuAccessFlag CPUAccessFlag => CpuAccessFlag.Read;

    protected override Usage Usage => Usage.Staging;

    public StagingTexture2D(Texture2D<T> baseTexture)
        : base(baseTexture.Device, false, true, false)
    {
        Format = baseTexture.Format;

        Width = baseTexture.Width;

        Height = baseTexture.Height;

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
    }

    protected override void CreateView()
    {
        throw new InvalidOperationException("Cannot create a view on a StagingTexture");
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

    //internal Texture2D CreateStagingTexture()
    //{
    //    Texture2DDesc desc = GetTextureDescription();

    //    desc = desc with
    //    {
    //        Usage = Usage.Staging,
    //        CPUAccessFlags = (uint)CpuAccessFlag.Read,
    //        MipLevels = 1,
    //        BindFlags = (uint)BindFlag.None,
    //        SampleDesc = new SampleDesc(1, 0),
    //    };


    //    return new Texture2D(Device, desc);
    //}
}
