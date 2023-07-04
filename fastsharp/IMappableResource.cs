namespace FastSharp;

public interface IMappableResource
{
    public Span<T> MapWrite<T>(Span<T> span) where T : unmanaged;

    public ReadOnlySpan<T> MapRead<T>(Span<T> span) where T : unmanaged;
}
