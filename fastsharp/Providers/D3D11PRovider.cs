using Silk.NET.Direct3D11;
using Silk.NET.Windowing;

namespace FastSharp.Providers;

internal static class D3D11PRovider
{
    public static D3D11 D3D11 { get; private set; }

    static D3D11PRovider()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        D3D11 = D3D11.GetApi();
#pragma warning restore CS0618
    }
}
