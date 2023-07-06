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

    public unsafe void SetUnorderedAccessResource(int slot, Texture texture)
    {
        Device.GraphicsDeviceContext.CSSetUnorderedAccessViews((uint)slot, 1, texture.GraphicsUAV, (uint*)null);
    }

    public unsafe void SetUnorderedAccessResource<T>(int slot, RWStructuredBuffer<T> buffer)
        where T : unmanaged
    {
        Device.GraphicsDeviceContext.CSSetUnorderedAccessViews((uint)slot, 1, buffer.GraphicsUAV, (uint*)null);
    }

    public unsafe void SetShaderResource(int slot, Texture texture)
    {
        Device.GraphicsDeviceContext.CSSetShaderResources((uint)slot, 1, texture.GraphicsSRV);
    }

    public unsafe void SetShaderResource<T>(int slot, StructuredBuffer<T> buffer)
        where T : unmanaged
    {
        Device.GraphicsDeviceContext.CSSetShaderResources((uint)slot, 1, buffer.GraphicsSRV);
    }

    public unsafe void SetShaderResource<T>(int slot, ShaderResourceBuffer<T> buffer)
        where T : unmanaged
    {
        Device.GraphicsDeviceContext.CSSetShaderResources((uint)slot, 1, buffer.GraphicsSRV);
    }
}
