using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class RWTexture2D<T> : Texture2D<T>
    where T : unmanaged
{
    protected override BindFlag BindFlag => BindFlag.UnorderedAccess;

    public RWTexture2D(Device device, Format format, int width, int height)
        : base(device, format, width, height, false, false)
    {

    }

    public RWTexture2D(Device device, Format format, int width, int height, ReadOnlySpan<T> initialData)
        : base(device, format, width, height, initialData, false, false)
    {

    }

    protected override void CreateView()
    {
        CreateUAV(UavDimension.Texture2D);
    }
}
