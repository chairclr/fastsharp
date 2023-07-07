using System.Runtime.InteropServices;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public partial class Device
{
    //public Texture2D CreateTexture2D(int width, int height, Format format, Usage usage, BindFlag bindFlag, CpuAccessFlag cpuAccessFlag)
    //{
    //    return new Texture2D(this, width, height, format, usage, bindFlag, cpuAccessFlag);
    //}

    //public Texture2D CreateTexture2D<T>(int width, int height, Format format, Usage usage, BindFlag bindFlag, CpuAccessFlag cpuAccessFlag, ReadOnlySpan<T> initialData)
    //    where T : unmanaged
    //{
    //    return new Texture2D(this, width, height, MemoryMarshal.AsBytes(initialData), format, usage, bindFlag, cpuAccessFlag);
    //}

    //public Texture2D CreateRWTexture2D(int width, int height)
    //{
    //    return new Texture2D(this, width, height, bindFlag: BindFlag.UnorderedAccess);
    //}

    //public Texture2D CreateRWTexture2D(int width, int height, ReadOnlySpan<Rgba32> data)
    //{
    //    return new Texture2D(this, width, height, MemoryMarshal.AsBytes(data), bindFlag: BindFlag.UnorderedAccess);
    //}

    //public Texture2D CreateRWTexture2D<T>(int width, int height, Format format, ReadOnlySpan<T> data)
    //    where T : unmanaged
    //{
    //    return new Texture2D(this, width, height, MemoryMarshal.AsBytes(data), format: format, bindFlag: BindFlag.UnorderedAccess);
    //}

    //public Texture2D CreateShaderResourceTexture2D(int width, int height)
    //{
    //    return new Texture2D(this, width, height, bindFlag: BindFlag.ShaderResource);
    //}

    //public Texture2D CreateShaderResourceTexture2D(int width, int height, ReadOnlySpan<Rgba32> data)
    //{
    //    return new Texture2D(this, width, height, MemoryMarshal.AsBytes(data), bindFlag: BindFlag.ShaderResource);
    //}

    //public Texture2D CreateShaderResourceTexture2D<T>(int width, int height, Format format, ReadOnlySpan<T> data)
    //    where T : unmanaged
    //{
    //    return new Texture2D(this, width, height, MemoryMarshal.AsBytes(data), format: format, bindFlag: BindFlag.ShaderResource);
    //}

    //public Texture2D CreateImmutableTexture2D(int width, int height, ReadOnlySpan<Rgba32> data)
    //{
    //    return new Texture2D(this, width, height, MemoryMarshal.AsBytes(data), usage: Usage.Immutable, bindFlag: BindFlag.ShaderResource);
    //}

    //public Texture2D CreateImmutableTexture2D<T>(int width, int height, Format format, ReadOnlySpan<T> data)
    //    where T : unmanaged
    //{
    //    return new Texture2D(this, width, height, MemoryMarshal.AsBytes(data), format: format, usage: Usage.Immutable, bindFlag: BindFlag.ShaderResource);
    //}

    ///// <summary>
    ///// Creates a copy of a <see cref="Texture2D"/> for staging purposes
    ///// </summary>
    ///// <param name="target">the texture to create a copy of</param>
    ///// <returns>A copy of <paramref name="target"/> for use in staging</returns>
    //public Texture2D CreateStagingCopy(Texture2D target)
    //{
    //    return target.CreateStagingTexture();
    //}
}
