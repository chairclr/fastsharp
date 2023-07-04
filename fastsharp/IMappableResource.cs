namespace FastSharp;

public interface IMappableResource
{
    public Span<T> MapWrite<T>(int subresource = 0) where T : unmanaged;

    public ReadOnlySpan<T> MapRead<T>(int subresource = 0) where T : unmanaged;

    public void Unmap(int subresource = 0);
}
