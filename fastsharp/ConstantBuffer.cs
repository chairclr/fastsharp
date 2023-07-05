namespace FastSharp;

public class ConstantBuffer<T> : Buffer<T>
    where T : unmanaged
{
    public ConstantBuffer(Device device)
        : base(device)
    {

    }
}
