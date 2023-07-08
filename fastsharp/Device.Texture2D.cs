using Silk.NET.DXGI;

namespace FastSharp;

public partial class Device
{
    public Texture2D<T> CreateTexture2D<T>(Format format, int width, int height, bool cpuWritable = false)
        where T : unmanaged
    {
        return new Texture2D<T>(this, format, width, height, false, cpuWritable);
    }

    public Texture2D<T> CreateTexture2D<T>(Format format, int width, int height, ReadOnlySpan<T> initialData, bool cpuWritable = false)
        where T : unmanaged
    {
        return new Texture2D<T>(this, format, width, height, initialData, false, cpuWritable);
    }

    public RWTexture2D<T> CreateRWTexture2D<T>(Format format, int width, int height)
        where T : unmanaged
    {
        return new RWTexture2D<T>(this, format, width, height);
    }

    public RWTexture2D<T> CreateRWTexture2D<T>(Format format, int width, int height, ReadOnlySpan<T> initialData)
        where T : unmanaged
    {
        return new RWTexture2D<T>(this, format, width, height, initialData);
    }

    public Texture2D<T> CreateImmutableTexture2D<T>(Format format, int width, int height, ReadOnlySpan<T> initialData)
        where T : unmanaged
    {
        return new Texture2D<T>(this, format, width, height, initialData, true);
    }

    public StagingTexture2D<T> CreateStagingTexture2D<T>(Texture2D<T> baseTexture)
        where T : unmanaged
    {
        return new StagingTexture2D<T>(baseTexture);
    }
}
