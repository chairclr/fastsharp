using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class ShaderResourceBuffer<T> : CPUAccessibleBuffer<T>, IMappableResource<T>
    where T : unmanaged
{
    public Format Format { get; private set; }

    internal ComPtr<ID3D11ShaderResourceView> GraphicsSRV = default;

    public ShaderResourceBuffer(Device device, Format format, int length, bool readable, bool writable)
        : base(device, readable, writable)
    {
        Format = format;

        Length = (uint)length;

        BufferDesc bufferDesc = new BufferDesc()
        {
            BindFlags = (uint)BindFlag.ShaderResource,
            ByteWidth = Size,
            Usage = base.Usage,
            CPUAccessFlags = (uint)base.CpuAccessFlag
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateBuffer(bufferDesc, (SubresourceData*)null, ref GraphicsBuffer));

        CacheSRV();
    }

    public ShaderResourceBuffer(Device device, Format format, ReadOnlySpan<T> initialData, bool readable, bool writable)
        : base(device, readable, writable)
    {
        Format = format;

        Length = (uint)initialData.Length;

        BufferDesc bufferDesc = new BufferDesc()
        {
            BindFlags = (uint)BindFlag.ShaderResource,
            ByteWidth = Size,
            Usage = base.Usage,
            CPUAccessFlags = (uint)base.CpuAccessFlag
        };

        SubresourceData subresourceData = new SubresourceData()
        {
            PSysMem = Unsafe.AsPointer(ref initialData.DangerousGetReference())
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateBuffer(bufferDesc, subresourceData, ref GraphicsBuffer));

        CacheSRV();
    }

    private void CacheSRV()
    {
        ShaderResourceViewDesc shaderResourceViewDesc = new ShaderResourceViewDesc()
        {
            Format = Format,
            ViewDimension = D3DSrvDimension.D3D101SrvDimensionBuffer
        };

        shaderResourceViewDesc.Buffer.FirstElement = 0;
        shaderResourceViewDesc.Buffer.NumElements = Length;

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateShaderResourceView(GraphicsBuffer, shaderResourceViewDesc, ref GraphicsSRV));
    }
}
