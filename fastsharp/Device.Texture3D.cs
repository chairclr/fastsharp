using System.Runtime.InteropServices;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public partial class Device
{
    public Texture3D CreateTexture3D(int width, int height, int depth, Format format, Usage usage, BindFlag bindFlag, CpuAccessFlag cpuAccessFlag)
    {
        return new Texture3D(this, width, height, depth, format, usage, bindFlag, cpuAccessFlag);
    }

    public Texture3D CreateTexture3D<T>(int width, int height, int depth, Format format, Usage usage, BindFlag bindFlag, CpuAccessFlag cpuAccessFlag, Span<T> initialData)
        where T : unmanaged
    {
        return new Texture3D(this, width, height, depth, MemoryMarshal.AsBytes(initialData), format, usage, bindFlag, cpuAccessFlag);
    }

    public Texture3D CreateRWTexture3D(int width, int height, int depth)
    {
        return new Texture3D(this, width, height, depth, bindFlag: BindFlag.UnorderedAccess);
    }

    public Texture3D CreateRWTexture3D(int width, int height, int depth, Span<Rgba32> data)
    {
        return new Texture3D(this, width, height, depth, MemoryMarshal.AsBytes(data), bindFlag: BindFlag.UnorderedAccess);
    }

    public Texture3D CreateRWTexture3D<T>(int width, int height, int depth, Format format, Span<T> data)
        where T : unmanaged
    {
        return new Texture3D(this, width, height, depth, MemoryMarshal.AsBytes(data), format: format, bindFlag: BindFlag.UnorderedAccess);
    }

    public Texture3D CreateShaderResourceTexture3D(int width, int height, int depth)
    {
        return new Texture3D(this, width, height, depth, bindFlag: BindFlag.ShaderResource);
    }

    public Texture3D CreateShaderResourceTexture3D(int width, int height, int depth, Span<Rgba32> data)
    {
        return new Texture3D(this, width, height, depth, MemoryMarshal.AsBytes(data), bindFlag: BindFlag.ShaderResource);
    }

    public Texture3D CreateShaderResourceTexture3D<T>(int width, int height, int depth, Format format, Span<T> data)
        where T : unmanaged
    {
        return new Texture3D(this, width, height, depth, MemoryMarshal.AsBytes(data), format: format, bindFlag: BindFlag.ShaderResource);
    }

    public Texture3D CreateImmutableTexture3D(int width, int height, int depth, Span<Rgba32> data)
    {
        return new Texture3D(this, width, height, depth, MemoryMarshal.AsBytes(data), usage: Usage.Immutable, bindFlag: BindFlag.ShaderResource);
    }

    public Texture3D CreateImmutableTexture3D<T>(int width, int height, int depth, Format format, Span<T> data)
        where T : unmanaged
    {
        return new Texture3D(this, width, height, depth, MemoryMarshal.AsBytes(data), format: format, usage: Usage.Immutable, bindFlag: BindFlag.ShaderResource);
    }

    /// <summary>
    /// Creates a copy of a <see cref="Texture3D"/> for staging purposes
    /// </summary>
    /// <param name="target">the texture to create a copy of</param>
    /// <returns>A copy of <paramref name="target"/> for use in staging</returns>
    public Texture3D CreateStagingCopy(Texture3D target)
    {
        return target.CreateStagingTexture();
    }
}
