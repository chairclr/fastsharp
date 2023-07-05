using Silk.NET.Direct3D11;

namespace FastSharp.Providers;

internal static class D3D11Provider
{
    public static D3D11 D3D11 { get; private set; }

    static D3D11Provider()
    {
        D3D11 = D3D11.GetApi(null);
    }
}
