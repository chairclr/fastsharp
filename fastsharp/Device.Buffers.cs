namespace FastSharp;

public partial class Device
{
    public ConstantBuffer<T> CreateConstantBuffer<T>()
        where T : unmanaged
    {
        return new ConstantBuffer<T>(this);
    }

    public ConstantBuffer<T> CreateConstantBuffer<T>(in T initialData)
        where T : unmanaged
    {
        return new ConstantBuffer<T>(this, initialData);
    }

    public StructuredBuffer<T> CreateStructuredBuffer<T>(int length, bool cpuReadable = false, bool cpuWritable = false)
        where T : unmanaged
    {
        return new StructuredBuffer<T>(this, length, cpuReadable, cpuWritable);
    }

    public StructuredBuffer<T> CreateStructuredBuffer<T>(ReadOnlySpan<T> initialData, bool cpuReadable = false, bool cpuWritable = false)
        where T : unmanaged
    {
        return new StructuredBuffer<T>(this, initialData, cpuReadable, cpuWritable);
    }

    public RWStructuredBuffer<T> CreateRWStructuredBuffer<T>(int length, bool cpuReadable = false, bool cpuWritable = false)
        where T : unmanaged
    {
        return new RWStructuredBuffer<T>(this, length, cpuReadable, cpuWritable);
    }

    public RWStructuredBuffer<T> CreateRWStructuredBuffer<T>(ReadOnlySpan<T> initialData, bool cpuReadable = false, bool cpuWritable = false)
        where T : unmanaged
    {
        return new RWStructuredBuffer<T>(this, initialData, cpuReadable, cpuWritable);
    }
}
