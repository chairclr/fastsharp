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

        using Texture1D texture = Device.CreateImmutableTexture1D(4, colors);

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

        using Texture1D texture = Device.CreateImmutableTexture1D(4, inputColors);

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

        using Texture1D texture = Device.CreateImmutableTexture1D(4, inputColors);

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

        using Texture2D texture = Device.CreateImmutableTexture2D(2, 2, colors);

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

        using Texture2D texture = Device.CreateImmutableTexture2D(2, 2, inputColors);

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

        using Texture2D texture = Device.CreateImmutableTexture2D(2, 2, inputColors);

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
            Color.Blue
        };

        using Texture3D texture = Device.CreateImmutableTexture3D(1, 1, 1, colors);

        Assert.Pass();
    }

    [Test]
    public void ReadImmutableTexture3D()
    {
        Rgba32[] inputColors = new Rgba32[]
        {
            Color.Red, Color.Green,
            Color.Blue
        };

        using Texture3D texture = Device.CreateImmutableTexture3D(1, 1, 1, inputColors);

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
            Color.Blue
        };

        using Texture3D texture = Device.CreateImmutableTexture3D(1, 1, 1, inputColors);

        Assert.Throws<Exception>(() =>
        {
            texture.MapWrite<Rgba32>();
        });
    }
}
