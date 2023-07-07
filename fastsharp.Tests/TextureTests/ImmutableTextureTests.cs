using CommunityToolkit.HighPerformance;
using FastSharp.Shaders;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FastSharp.TextureTests;

public class ImmutableTextureTests
{
    public Device Device;

    public ComputeShader ComputeShader;

    [SetUp]
    public void SetUp()
    {
        Device = new Device();

        ComputeShader = Device.CompileShaderFromFile<ComputeShader>("TextureTests/ImmutableComputeShader.hlsl", "CSMain", ShaderProfile.CS5_0);
    }

    [TearDown]
    public void TearDown()
    {
        Device.Dispose();
    }

    [Test]
    public void CreateImmutableTexture1D()
    {
        Rgba32[] colors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White
        };

        using Texture1D<Rgba32> texture = Device.CreateImmutableTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, colors);

        Assert.Pass();
    }

    [Test]
    public void CreateImmutableTexture2D()
    {
        Rgba32[] colors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White
        };

        using Texture2D<Rgba32> texture = Device.CreateImmutableTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 2, colors);

        Assert.Pass();
    }

    [Test]
    public void CreateImmutableTexture3D()
    {
        Rgba32[] colors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White,
            Color.Black, Color.White,
            Color.AliceBlue, Color.BlanchedAlmond
        };

        using Texture3D<Rgba32> texture = Device.CreateImmutableTexture3D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 2, 2, colors);

        Assert.Pass();
    }

    [Test]
    public void TestInput()
    {
        Rgba32[] inputColors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White
        };

        using Texture2D<Rgba32> immutableTexture = Device.CreateImmutableTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 2, inputColors);
        using Texture2D<Rgba32> rwTexture = Device.CreateRWTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 2);
        using StagingTexture2D<Rgba32> stagingTexture = Device.CreateStagingTexture2D(rwTexture);

        ComputeShader.SetShaderResource(0, immutableTexture);
        ComputeShader.SetUnorderedAccessResource(0, rwTexture);
        ComputeShader.Dispatch(1, 1, 1);

        rwTexture.CopyTo(stagingTexture);

        Rgba32[,] values = stagingTexture.Read();

        Assert.That(values.AsSpan().ToArray(), Is.EquivalentTo(inputColors));
    }
}
