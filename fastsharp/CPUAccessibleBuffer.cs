using System.Runtime.InteropServices;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;

namespace FastSharp;

public unsafe abstract class CPUAccessibleBuffer<T> : Buffer<T>, IMappableResource<T>
    where T : unmanaged
{
    public bool Writable { get; private set; }

    public bool Readable { get; private set; }

    protected Usage Usage => (Readable || Writable) ? Usage.Dynamic : Usage.Default;

    protected CpuAccessFlag CpuAccessFlag => (Writable ? CpuAccessFlag.Write : CpuAccessFlag.None) | (Readable ? CpuAccessFlag.Read : CpuAccessFlag.None);

    protected CPUAccessibleBuffer(Device device, bool readable, bool writable)
        : base(device)
    {
        Writable = writable;

        Readable = readable;
    }

    public Span<T> MapWrite(int subresource = 0)
    {
        if (!Writable)
        {
            throw new Exception("Buffer not writable");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsBuffer, (uint)subresource, Map.WriteDiscard, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource", Marshal.GetExceptionForHR(hr));
        }

        return new Span<T>(mappedSubresource.PData, (int)Size);
    }

    public ReadOnlySpan<T> MapRead(int subresource = 0)
    {
        if (!Readable)
        {
            throw new Exception("Buffer not readable");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsBuffer, (uint)subresource, Map.Read, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource", Marshal.GetExceptionForHR(hr));
        }

        return new ReadOnlySpan<T>(mappedSubresource.PData, (int)Size);
    }

    public Span<T> MapReadWrite(int subresource = 0)
    {
        if (!Readable || !Writable)
        {
            throw new Exception("Buffer not read/writable");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsBuffer, (uint)subresource, Map.ReadWrite, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource", Marshal.GetExceptionForHR(hr));
        }

        return new Span<T>(mappedSubresource.PData, (int)Size);
    }

    public void WriteData(Span<T> data, int subresource = 0)
    {
        data.CopyTo(MapWrite(subresource));

        Unmap(subresource);
    }

    public void Unmap(int subresource = 0)
    {
        if (!(Readable || Writable))
        {
            throw new Exception("Cannot unmap an unreadable or unwritable buffer");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        Device.GraphicsDeviceContext.Unmap(GraphicsBuffer, (uint)subresource);
    }
}
