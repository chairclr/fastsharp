using Silk.NET.Core.Native;
using System.Runtime.CompilerServices;
using Silk.NET.Direct3D11;
using CommunityToolkit.HighPerformance;

namespace FastSharp;

public unsafe class ConstantBuffer<T> : Buffer<T>
    where T : unmanaged
{
    // The Length of constant buffers should always be one
    public override uint Length => 1;

    // Constant buffers should be aligned to 16 byte boundaries
    // https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-packing-rules
    public override uint Stride => AlignmentUtility.Align(base.Stride, 16u);

    public T Data;

    public ConstantBuffer(Device device, T initialData)
        : base(device)
    {
        Data = initialData;

        BufferDesc bufferDesc = new BufferDesc()
        {
            Usage = Usage.Dynamic,
            BindFlags = (uint)BindFlag.ConstantBuffer,
            ByteWidth = Size,
            CPUAccessFlags = (uint)CpuAccessFlag.Write
        };

        Span<byte> data = stackalloc byte[(int)Stride];
        Unsafe.Copy(Unsafe.AsPointer(ref data.DangerousGetReference()), ref Data);

        SubresourceData subresourceData = new SubresourceData()
        {
            PSysMem = Unsafe.AsPointer(ref data.DangerousGetReference())
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateBuffer(bufferDesc, subresourceData, ref GraphicsBuffer));
    }

    public ConstantBuffer(Device device)
        : this(device, default)
    {

    }

    /// <summary>
    /// Copies the buffer to the GPU.
    /// This should be called every time you change Data and need to render on the GPU.
    /// </summary>
    public unsafe void WriteData()
    {
        MappedSubresource mappedSubresource = new MappedSubresource();

        SilkMarshal.ThrowHResult(Device.GraphicsDeviceContext.Map(GraphicsBuffer, 0, Map.WriteDiscard, 0, ref mappedSubresource));

        Unsafe.Copy(mappedSubresource.PData, ref Data);

        // Set the trailing extra alignment memory to zero if necessary
        if (Unsafe.SizeOf<T>() != Stride)
        {
            Unsafe.InitBlock((void*)((nint)mappedSubresource.PData + Unsafe.SizeOf<T>()), 0, Stride - (uint)Unsafe.SizeOf<T>());
        }

        Device.GraphicsDeviceContext.Unmap(GraphicsBuffer, 0);
    }
}
