using Silk.NET.Direct3D.Compilers;

namespace FastSharp.Providers;

internal static class D3DCompilerProvider
{
    public static D3DCompiler D3DCompiler { get; private set; }

    static D3DCompilerProvider()
    {
        D3DCompiler = D3DCompiler.GetApi();
    }
}
