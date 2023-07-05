using System.Runtime.CompilerServices;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;

namespace FastSharp.Shaders;

public class ComputeShader : Shader<ID3D11ComputeShader>
{
    public ComputeShader(Device renderer)
        : base(renderer)
    {

    }

    internal unsafe override void Create()
    {
        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateComputeShader(ShaderData.GraphicsBlob.GetBufferPointer(), ShaderData.Size, ref Unsafe.NullRef<ID3D11ClassLinkage>(), ref GraphicsShader));
    }

    protected override unsafe void Bind()
    {
        Device.GraphicsDeviceContext.CSSetShader(GraphicsShader, (ID3D11ClassInstance*)null, 0);
    }

    public void Dispatch(uint threadGroupCountX, uint threadGroupCountY, uint threadGroupCountZ)
    {
        Bind();

        Device.GraphicsDeviceContext.Dispatch(threadGroupCountX, threadGroupCountY, threadGroupCountZ);
    }

    public unsafe void SetUnorderedAccessView(int slot, Texture texture)
    {
        Device.GraphicsDeviceContext.CSSetUnorderedAccessViews((uint)slot, 1, texture.GraphicsUAV, (uint*)null);
    }
}
