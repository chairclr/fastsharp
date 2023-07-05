using System.Runtime.InteropServices;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public partial class Device
{
    public Texture1D CreateTexture1D(int length, Format format, Usage usage, BindFlag bindFlag, CpuAccessFlag cpuAccessFlag)
    {
        return new Texture1D(this, length, format, usage, bindFlag, cpuAccessFlag);
    }

    public Texture1D CreateTexture1D<T>(int length, Format format, Usage usage, BindFlag bindFlag, CpuAccessFlag cpuAccessFlag, Span<T> initialData)
        where T : unmanaged
    {
        return new Texture1D(this, length, MemoryMarshal.AsBytes(initialData), format, usage, bindFlag, cpuAccessFlag);
    }

    public Texture1D CreateRWTexture1D(int length)
    {
        return new Texture1D(this, length, bindFlag: BindFlag.UnorderedAccess);
    }

    public Texture1D CreateRWTexture1D(int length, Span<Rgba32> data)
    {
        return new Texture1D(this, length, MemoryMarshal.AsBytes(data), bindFlag: BindFlag.UnorderedAccess);
    }

    public Texture1D CreateRWTexture1D<T>(int length, Format format, Span<T> data)
        where T : unmanaged
    {
        return new Texture1D(this, length, MemoryMarshal.AsBytes(data), format: format, bindFlag: BindFlag.UnorderedAccess);
    }

    public Texture1D CreateShaderResourceTexture1D(int length)
    {
        return new Texture1D(this, length, bindFlag: BindFlag.ShaderResource);
    }

    public Texture1D CreateShaderResourceTexture1D(int length, Span<Rgba32> data)
    {
        return new Texture1D(this, length, MemoryMarshal.AsBytes(data), bindFlag: BindFlag.ShaderResource);
    }

    public Texture1D CreateShaderResourceTexture1D<T>(int length, Format format, Span<T> data)
        where T : unmanaged
    {
        return new Texture1D(this, length, MemoryMarshal.AsBytes(data), format: format, bindFlag: BindFlag.ShaderResource);
    }

    public Texture1D CreateImmutableTexture1D(int length, Span<Rgba32> data)
    {
        return new Texture1D(this, length, MemoryMarshal.AsBytes(data), usage: Usage.Immutable, bindFlag: BindFlag.ShaderResource);
    }

    public Texture1D CreateImmutableTexture1D<T>(int length, Format format, Span<T> data)
        where T : unmanaged
    {
        return new Texture1D(this, length, MemoryMarshal.AsBytes(data), format: format, usage: Usage.Immutable, bindFlag: BindFlag.ShaderResource);
    }

    /// <summary>
    /// Creates a copy of a <see cref="Texture1D"/> for staging purposes
    /// </summary>
    /// <param name="target">the texture to create a copy of</param>
    /// <returns>A copy of <paramref name="target"/> for use in staging</returns>
    public Texture1D CreateStagingCopy(Texture1D target)
    {
        return target.CreateStagingTexture();
    }
}
