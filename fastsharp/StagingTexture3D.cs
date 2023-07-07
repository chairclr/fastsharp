using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class StagingTexture3D<T> : Texture<ID3D11Texture3D>
    where T : unmanaged
{
    public int Width { get; private set; } = 0;

    public int Height { get; private set; } = 0;

    public int Depth { get; private set; } = 0;

    protected override int Size => Width * Height * Depth;

    protected override BindFlag BindFlag => BindFlag.None;

    protected override CpuAccessFlag CPUAccessFlag => CpuAccessFlag.Read;

    protected override Usage Usage => Usage.Staging;

    public StagingTexture3D(Texture3D<T> baseTexture)
        : base(baseTexture.Device, false, true, false)
    {
        Format = baseTexture.Format;

        Width = baseTexture.Width;

        Height = baseTexture.Height;

        Depth = baseTexture.Depth;

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
    }

    protected override void CreateView()
    {
        throw new InvalidOperationException("Cannot create a view on a StagingTexture");
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

    //internal Texture3D CreateStagingTexture()
    //{
    //    Texture3DDesc desc = GetTextureDescription();

    //    desc = desc with
    //    {
    //        Usage = Usage.Staging,
    //        CPUAccessFlags = (uint)CpuAccessFlag.Read,
    //        MipLevels = 1,
    //        BindFlags = (uint)BindFlag.None,
    //        SampleDesc = new SampleDesc(1, 0),
    //    };


    //    return new Texture3D(Device, desc);
    //}
}
