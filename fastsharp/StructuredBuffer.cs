using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class StructuredBuffer<T> : CPUAccessibleBuffer<T>, IMappableResource<T>
    where T : unmanaged
{
    internal ComPtr<ID3D11ShaderResourceView> GraphicsSRV = default;

    public StructuredBuffer(Device device, int length, bool readable, bool writable)
        : base(device, readable, writable)
    {
        Length = (uint)length;

        BufferDesc bufferDesc = new BufferDesc()
        {
            BindFlags = (uint)BindFlag.ShaderResource,
            StructureByteStride = Stride,
            ByteWidth = Size,
            Usage = base.Usage,
            CPUAccessFlags = (uint)base.CpuAccessFlag,
            MiscFlags = (uint)ResourceMiscFlag.BufferStructured
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateBuffer(bufferDesc, (SubresourceData*)null, ref GraphicsBuffer));

        CacheSRV();
    }

    public StructuredBuffer(Device device, ReadOnlySpan<T> initialData, bool readable, bool writable)
        : base(device, readable, writable)
    {
        BufferDesc bufferDesc = new BufferDesc()
        {
            BindFlags = (uint)BindFlag.ShaderResource,
            StructureByteStride = Stride,
            ByteWidth = Size,
            Usage = (Readable || Writable) ? Usage.Dynamic : Usage.Default,
            CPUAccessFlags = (uint)((Writable ? CpuAccessFlag.Write : CpuAccessFlag.None) | (Readable ? CpuAccessFlag.Read : CpuAccessFlag.None)),
            MiscFlags = (uint)ResourceMiscFlag.BufferStructured
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
            Format = Format.FormatUnknown,
            ViewDimension = D3DSrvDimension.D3D101SrvDimensionBuffer
        };

        shaderResourceViewDesc.Buffer.FirstElement = 0;
        shaderResourceViewDesc.Buffer.NumElements = Length;

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateShaderResourceView(GraphicsBuffer, shaderResourceViewDesc, ref GraphicsSRV));
    }
}
