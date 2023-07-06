﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public unsafe class RWStructuredBuffer<T> : Buffer<T>, IMappableResource<T>
    where T : unmanaged
{
    public bool Writable { get; private set; }

    public bool Readable { get; private set; }

    internal ComPtr<ID3D11UnorderedAccessView> GraphicsUAV = default;

    public RWStructuredBuffer(Device device, int length, bool writable, bool readable)
        : base(device)
    {
        Writable = writable;

        Readable = readable;

        Length = (uint)length;

        BufferDesc bufferDesc = new BufferDesc()
        {
            BindFlags = (uint)BindFlag.UnorderedAccess,
            StructureByteStride = Stride,
            ByteWidth = Size,
            Usage = (Readable || Writable) ? Usage.Dynamic : Usage.Default,
            CPUAccessFlags = (uint)((Writable ? CpuAccessFlag.Write : CpuAccessFlag.None) | (Readable ? CpuAccessFlag.Read : CpuAccessFlag.None)),
            MiscFlags = (uint)ResourceMiscFlag.BufferStructured
        };

        SilkMarshal.ThrowHResult(Device.GraphicsDevice.CreateBuffer(bufferDesc, (SubresourceData*)null, ref GraphicsBuffer));

        CacheUAV();
    }

    public RWStructuredBuffer(Device device, ReadOnlySpan<T> initialData, bool writable, bool readable)
        : base(device)
    {
        Writable = writable;

        Readable = readable;

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

    public Span<T> MapWrite(int subresource = 0)
    {
        if (!Writable)
        {
            throw new Exception("Buffer not writable");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsBuffer, (uint)subresource, Map.WriteDiscard, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource", Marshal.GetExceptionForHR(hr));
        }

        return new Span<T>(mappedSubresource.PData, (int)Size);
    }

    public ReadOnlySpan<T> MapRead(int subresource = 0)
    {
        if (!Readable)
        {
            throw new Exception("Buffer not readable");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsBuffer, (uint)subresource, Map.Read, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource", Marshal.GetExceptionForHR(hr));
        }

        return new ReadOnlySpan<T>(mappedSubresource.PData, (int)Size);
    }

    public Span<T> MapReadWrite(int subresource = 0)
    {
        if (!Readable || !Writable)
        {
            throw new Exception("Buffer not read/writable");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        MappedSubresource mappedSubresource = default;

        int hr = Device.GraphicsDeviceContext.Map(GraphicsBuffer, (uint)subresource, Map.ReadWrite, 0, ref mappedSubresource);

        if (!HResult.IndicatesSuccess(hr))
        {
            throw new Exception("Failed to map subresource", Marshal.GetExceptionForHR(hr));
        }

        return new Span<T>(mappedSubresource.PData, (int)Size);
    }

    public void WriteData(Span<T> data, int subresource = 0)
    {
        data.CopyTo(MapWrite(subresource));

        Unmap(subresource);
    }

    public void Unmap(int subresource = 0)
    {
        if (!(Readable || Writable))
        {
            throw new Exception("Cannot unmap an unreadable or unwritable buffer");
        }

        if (subresource < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subresource), "subresource must be positive");
        }

        Device.GraphicsDeviceContext.Unmap(GraphicsBuffer, (uint)subresource);
    }
}