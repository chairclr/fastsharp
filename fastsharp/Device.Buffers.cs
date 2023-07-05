using System.Runtime.InteropServices;

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
}
