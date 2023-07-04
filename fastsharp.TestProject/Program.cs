using FastSharp.Shaders;

namespace FastSharp.TestProject;

internal class Program
{
    static void Main(string[] args)
    {
        using Device device = new Device();

        using ComputeShader computeShader = device.CompileShaderFromFile<ComputeShader>("Shaders/TestComputeShader.hlsl", "CSMain", ShaderProfile.CS5_0);

        using Texture2D texture = device.CreateTexture2D(1024, 1024);

        // Calculate number of thread groups to process entire image
        int x = (int)Math.Ceiling(texture.Width / 16f);
        int y = (int)Math.Ceiling(texture.Height / 16f);

        computeShader.Dispatch((uint)x, (uint)y, 0);
    }
}
