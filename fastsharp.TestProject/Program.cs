using System.Numerics;
using FastSharp.Shaders;
using Silk.NET.DXGI;
using SixLabors.ImageSharp.PixelFormats;

namespace FastSharp.TestProject;

internal class Program
{
    static void Main(string[] args)
    {
        using Device device = new Device();

        // Only for debugging
#if DEBUG
        if (RenderDocUtil.RenderDocEnabled)
        {
            RenderDocUtil.StartFrameCapture();

            Thread.Sleep(1000);
        }
#endif

        using ComputeShader computeShader = device.CompileShaderFromFile<ComputeShader>("Shaders/TestComputeShader.hlsl", "CSMain", ShaderProfile.CS5_0);

        using RWTexture2D<Rgba32> texture = new RWTexture2D<Rgba32>(device, Format.FormatR8G8B8A8Unorm, 1024, 1024);
        using StagingTexture2D<Rgba32> stagingTexture = new StagingTexture2D<Rgba32>(texture);
        //using Texture2D<Rgba32> stagingTexture = device.CreateStagingCopy(texture);

        using Texture2D<Rgba32> immutableTexture = new Texture2D<Rgba32>(device, Format.FormatR8G8B8A8Unorm, 2, 2, new Rgba32[]
        {
            new Rgba32(1.0f, 0.0f, 0.0f), new Rgba32(0.0f, 1.0f, 1.0f),
            new Rgba32(0.0f, 1.0f, 0.0f), new Rgba32(1.0f, 0.0f, 1.0f),
        }, immutable: true);


        using RWTexture1D<Rgba32> cool1DTexture = new RWTexture1D<Rgba32>(device, Format.FormatR8G8B8A8Unorm, 2048);

        using RWStructuredBuffer<TestStruct> structuredBuffer = new RWStructuredBuffer<TestStruct>(device, coolStructs, false, false);

        Vector4[] values = new Vector4[] { new Vector4(0f), new Vector4(1f) };

        using ShaderResourceBuffer<Vector4> shaderResourceBuffer = new ShaderResourceBuffer<Vector4>(device, Format.FormatR32G32B32A32Float, values, true, true);

        ReadOnlySpan<Vector4> readValues = shaderResourceBuffer.MapRead();

        shaderResourceBuffer.Unmap();

        Vector4[] newValues = new Vector4[] { new Vector4(0.5f), new Vector4(1f) };

        shaderResourceBuffer.WriteData(newValues);

        // Calculate number of thread groups to process entire image
        int x = (int)Math.Ceiling(texture.Width / 16f);
        int y = (int)Math.Ceiling(texture.Height / 16f);

        computeShader.SetShaderResource(0, immutableTexture);
        computeShader.SetShaderResource(1, shaderResourceBuffer);
        computeShader.SetUnorderedAccessResource(0, texture);
        computeShader.SetUnorderedAccessResource(1, cool1DTexture);
        computeShader.SetUnorderedAccessResource(2, structuredBuffer);
        computeShader.Dispatch((uint)x, (uint)y, 1);

        texture.CopyTo(stagingTexture);

        ReadOnlySpan<Rgba32> textureSpan = stagingTexture.MapRead<Rgba32>();

        for (int i = 0; i < 20; i++)
        {
            Console.WriteLine($"{textureSpan[i].ToHex()}");
        }

#if DEBUG
        if (RenderDocUtil.RenderDocEnabled)
        {
            Thread.Sleep(1000);

            RenderDocUtil.EndFrameCapture();
        }
#endif

        Console.Read();
    }

    private struct TestStruct
    {
        public int X;
        public int Length;
        int __padding1;
        int __padding2;
    }
}
