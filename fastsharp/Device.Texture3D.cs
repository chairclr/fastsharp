using System.Runtime.InteropServices;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public partial class Device
{
    public Texture3D<T> CreateTexture3D<T>(Format format, int width, int height, int depth, bool cpuWritable = false)
        where T : unmanaged
    {
        return new Texture3D<T>(this, format, width, height, depth, false, cpuWritable);
    }

    public Texture3D<T> CreateTexture3D<T>(Format format, int width, int height, int depth, ReadOnlySpan<T> initialData, bool cpuWritable = false)
        where T : unmanaged
    {
        return new Texture3D<T>(this, format, width, height, depth, initialData, false, cpuWritable);
    }

    public RWTexture3D<T> CreateRWTexture3D<T>(Format format, int width, int height, int depth)
        where T : unmanaged
    {
        return new RWTexture3D<T>(this, format, width, height, depth);
    }

    public RWTexture3D<T> CreateRWTexture3D<T>(Format format, int width, int height, int depth, ReadOnlySpan<T> initialData)
        where T : unmanaged
    {
        return new RWTexture3D<T>(this, format, width, height, depth, initialData);
    }

    public Texture3D<T> CreateImmutableTexture3D<T>(Format format, int width, int height, int depth, ReadOnlySpan<T> initialData)
        where T : unmanaged
    {
        return new Texture3D<T>(this, format, width, height, depth, initialData, true);
    }

    public StagingTexture3D<T> CreateStagingTexture3D<T>(Texture3D<T> baseTexture)
        where T : unmanaged
    {
        return new StagingTexture3D<T>(baseTexture);
    }
}
