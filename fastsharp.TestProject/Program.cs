using FastSharp.Shaders;
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

        using Texture2D texture = device.CreateUnorderedAccessTexture2D(1024, 1024);
        using Texture2D stagingTexture = device.CreateStagingCopy(texture);

        using Texture2D immutableTexture = device.CreateImmutableTexture2D(2, 2, new Rgba32[]
        {
            new Rgba32(1.0f, 0.0f, 0.0f), new Rgba32(0.0f, 1.0f, 1.0f),
            new Rgba32(0.0f, 1.0f, 0.0f), new Rgba32(1.0f, 0.0f, 1.0f),
        });

        using Texture1D cool1DTexture = device.CreateUnorderedAccessTexture1D(2048);

        // Calculate number of thread groups to process entire image
        int x = (int)Math.Ceiling(texture.Width / 16f);
        int y = (int)Math.Ceiling(texture.Height / 16f);

        computeShader.SetShaderResource(0, immutableTexture);
        computeShader.SetUnorderedAccessResource(0, texture);
        computeShader.SetUnorderedAccessResource(1, cool1DTexture);
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
}
