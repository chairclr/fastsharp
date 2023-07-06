namespace FastSharp;

public interface IMappableResource
{
    public Span<T> MapWrite<T>(int subresource = 0)
        where T : unmanaged;

    public ReadOnlySpan<T> MapRead<T>(int subresource = 0)
        where T : unmanaged;

    public Span<T> MapReadWrite<T>(int subresource = 0)
        where T : unmanaged;

    public void WriteData<T>(Span<T> data, int subresource = 0)
        where T : unmanaged;

    public void Unmap(int subresource = 0);
}

public interface IMappableResource<T>
    where T : unmanaged
{
    public Span<T> MapWrite(int subresource = 0);

    public ReadOnlySpan<T> MapRead(int subresource = 0);

    public Span<T> MapReadWrite(int subresource = 0);

    public void WriteData(Span<T> data, int subresource = 0);

    public void Unmap(int subresource = 0);
}