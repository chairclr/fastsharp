using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class RWTexture3D<T> : Texture3D<T>
    where T : unmanaged
{
    protected override BindFlag BindFlag => BindFlag.UnorderedAccess;

    public RWTexture3D(Device device, Format format, int width, int height, int depth)
        : base(device, format, width, height, depth, false, false)
    {

    }

    public RWTexture3D(Device device, Format format, int width, int height, int depth, ReadOnlySpan<T> initialData)
        : base(device, format, width, height, depth, initialData, false, false)
    {

    }

    protected override void CreateView()
    {
        CreateUAV(UavDimension.Texture3D);
    }
}
