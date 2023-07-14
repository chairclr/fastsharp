using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class RWTexture1D<T> : Texture1D<T>
    where T : unmanaged
{
    protected override BindFlag BindFlag => BindFlag.UnorderedAccess;

    public RWTexture1D(Device device, Format format, int length)
        : base(device, format, length, false, false)
    {

    }

    public RWTexture1D(Device device, Format format, ReadOnlySpan<T> initialData)
        : base(device, format, initialData, false, false)
    {

    }

    protected override void CreateView()
    {
        CreateUAV(UavDimension.Texture1D);
    }
}
