using Silk.NET.DXGI;

namespace FastSharp;

public partial class Device
{
    public Texture1D<T> CreateTexture1D<T>(Format format, int length, bool cpuWritable = false)
        where T : unmanaged
    {
        return new Texture1D<T>(this, format, length, false, cpuWritable);
    }

    public Texture1D<T> CreateTexture1D<T>(Format format, ReadOnlySpan<T> initialData, bool cpuWritable = false)
        where T : unmanaged
    {
        return new Texture1D<T>(this, format, initialData, false, cpuWritable);
    }

    public RWTexture1D<T> CreateRWTexture1D<T>(Format format, int length)
        where T : unmanaged
    {
        return new RWTexture1D<T>(this, format, length);
    }

    public RWTexture1D<T> CreateRWTexture1D<T>(Format format, ReadOnlySpan<T> initialData)
        where T : unmanaged
    {
        return new RWTexture1D<T>(this, format, initialData);
    }

    public Texture1D<T> CreateImmutableTexture1D<T>(Format format, ReadOnlySpan<T> initialData)
        where T : unmanaged
    {
        return new Texture1D<T>(this, format, initialData, true);
    }

    public StagingTexture1D<T> CreateStagingTexture1D<T>(Texture1D<T> baseTexture)
        where T : unmanaged
    {
        return new StagingTexture1D<T>(baseTexture);
    }
}
