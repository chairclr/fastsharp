using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class RWStructuredBuffer<T> : CPUAccessibleBuffer<T>, IMappableResource<T>
    where T : unmanaged
{
    internal ComPtr<ID3D11UnorderedAccessView> GraphicsUAV = default;

    public RWStructuredBuffer(Device device, int length, bool readable, bool writable)
        : base(device, readable, writable)
    {
        Length = (uint)length;

        BufferDesc bufferDesc = new BufferDesc()
        {
            BindFlags = (uint)BindFlag.UnorderedAccess,
            StructureByteStride = Stride,
            ByteWidth = Size,
            Usage = base.Usage,
            CPUAccessFlags = (uint)base.CpuAccessFlag,
            MiscFlags = (uint)ResourceMiscFlag.BufferStructured
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateBuffer(bufferDesc, (SubresourceData*)null, ref GraphicsBuffer));

        CacheUAV();
    }

    public RWStructuredBuffer(Device device, ReadOnlySpan<T> initialData, bool readable, bool writable)
        : base(device, readable, writable)
    {
        Length = (uint)initialData.Length;

        BufferDesc bufferDesc = new BufferDesc()
        {
            BindFlags = (uint)BindFlag.UnorderedAccess,
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

        CacheUAV();
    }

    private void CacheUAV()
    {
        UnorderedAccessViewDesc unorderedAccessViewDesc = new UnorderedAccessViewDesc()
        {
            Format = Format.FormatUnknown,
            ViewDimension = UavDimension.Buffer
        };

        unorderedAccessViewDesc.Buffer.FirstElement = 0;
        unorderedAccessViewDesc.Buffer.NumElements = Length;

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateUnorderedAccessView(GraphicsBuffer, unorderedAccessViewDesc, ref GraphicsUAV));
    }
}
