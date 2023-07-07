using Silk.NET.DXGI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FastSharp.TextureTests;

public class ImmutableTextureTests
{
    public Device Device;

    [SetUp]
    public void SetUp()
    {
        Device = new Device();
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
    public void ReadImmutableTexture1D()
    {
        Rgba32[] inputColors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White
        };

        using Texture1D<Rgba32> texture = Device.CreateImmutableTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, inputColors);

        Assert.Throws<Exception>(() =>
        {
            texture.MapWrite<Rgba32>();
        });
    }

    [Test]
    public void WriteImmutableTexture1D()
    {
        Rgba32[] inputColors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White
        };

        using Texture1D<Rgba32> texture = Device.CreateImmutableTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, inputColors);

        Assert.Throws<Exception>(() =>
        {
            texture.MapWrite<Rgba32>();
        });
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
    public void ReadImmutableTexture2D()
    {
        Rgba32[] inputColors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White
        };

        using Texture2D<Rgba32> texture = Device.CreateImmutableTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 2, inputColors);

        Assert.Throws<Exception>(() =>
        {
            texture.MapWrite<Rgba32>();
        });
    }

    [Test]
    public void WriteImmutableTexture2D()
    {
        Rgba32[] inputColors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White
        };

        using Texture2D<Rgba32> texture = Device.CreateImmutableTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 2, inputColors);

        Assert.Throws<Exception>(() =>
        {
            texture.MapWrite<Rgba32>();
        });
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
    public void ReadImmutableTexture3D()
    {
        Rgba32[] inputColors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White,
            Color.Black, Color.White,
            Color.AliceBlue, Color.BlanchedAlmond
        };

        using Texture3D<Rgba32> texture = Device.CreateImmutableTexture3D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 2, 2, inputColors);

        Assert.Throws<Exception>(() =>
        {
            texture.MapWrite<Rgba32>();
        });
    }

    [Test]
    public void WriteImmutableTexture3D()
    {
        Rgba32[] inputColors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue, Color.White,
            Color.Black, Color.White,
            Color.AliceBlue, Color.BlanchedAlmond
        };

        using Texture3D<Rgba32> texture = Device.CreateImmutableTexture3D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 2, 2, inputColors);

        Assert.Throws<Exception>(() =>
        {
            texture.MapWrite<Rgba32>();
        });
    }
}
