using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Helpers;
using FastSharp.Shaders;
using Silk.NET.DXGI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FastSharp.TextureTests;

public class MapTextureTests
{
    public Device Device;

    public const int Texture2DWidth = 4;
    public const int Texture2DHeight = 5;

    public Color[] Texture2DInitialData = new Color[Texture2DWidth * Texture2DHeight]
    {
        Color.Red, Color.Green,         Color.Red, Color.Red,
        Color.Blue, Color.Blue,         Color.Blue, Color.Orange,
        Color.Orchid, Color.AliceBlue,  Color.Red, Color.AliceBlue,
        Color.Blue, Color.Orange,       Color.Blue, Color.Orange,
        Color.Yellow, Color.AliceBlue,  Color.Orchid, Color.Red,
    };

    public Rgba32[] Texture2DInitialData32 => Texture2DInitialData.Select(x => x.ToPixel<Rgba32>()).ToArray();
    public Rgba64[] Texture2DInitialData64 => Texture2DInitialData.Select(x => x.ToPixel<Rgba64>()).ToArray();

    public Color[] Texture1DInitialData = new Color[]
    {
        Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Orchid, Color.AliceBlue, Color.AntiqueWhite, Color.Aquamarine, Color.Brown, Color.CornflowerBlue
    };

    public Rgba32[] Texture1DInitialData32 => Texture1DInitialData.Select(x => x.ToPixel<Rgba32>()).ToArray();
    public Rgba64[] Texture1DInitialData64 => Texture1DInitialData.Select(x => x.ToPixel<Rgba64>()).ToArray();

    [OneTimeSetUp]
    public void SetUp()
    {
        Device = new Device();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        Device.Dispose();
    }

    [Test]
    public void TestReadStagingArray1DRgba32()
    {
        using Texture1D<Rgba32> baseTexture = Device.CreateTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, Texture1DInitialData32);
        using StagingTexture1D<Rgba32> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba32[] readValues = new Rgba32[Texture1DInitialData.Length];

        texture.Read(readValues);

        Assert.That(readValues, Is.EquivalentTo(Texture1DInitialData32));
    }

    [Test]
    public void TestReadStagingArray1DRgba64()
    {
        using Texture1D<Rgba64> baseTexture = Device.CreateTexture1D<Rgba64>(Format.FormatR16G16B16A16Unorm, Texture1DInitialData64);
        using StagingTexture1D<Rgba64> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba64[] readValues = new Rgba64[Texture1DInitialData.Length];

        texture.Read(readValues);

        Assert.That(readValues, Is.EquivalentTo(Texture1DInitialData64));
    }

    [Test]
    public void TestReadStagingNewArray1DRgba32()
    {
        using Texture1D<Rgba32> baseTexture = Device.CreateTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, Texture1DInitialData32);
        using StagingTexture1D<Rgba32> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba32[] readValues = texture.Read();

        Assert.That(readValues, Is.EquivalentTo(Texture1DInitialData32));
    }

    [Test]
    public void TestReadStagingNewArray1DRgba64()
    {
        using Texture1D<Rgba64> baseTexture = Device.CreateTexture1D<Rgba64>(Format.FormatR16G16B16A16Unorm, Texture1DInitialData64);
        using StagingTexture1D<Rgba64> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba64[] readValues = texture.Read();

        Assert.That(readValues, Is.EquivalentTo(Texture1DInitialData64));
    }

    [Test]
    public void TestMapReadStaging1DRgba32()
    {
        using Texture1D<Rgba32> baseTexture = Device.CreateTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, Texture1DInitialData32);
        using StagingTexture1D<Rgba32> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        ReadOnlySpan<Rgba32> readValues = texture.MapRead();

        Assert.That(readValues.ToArray(), Is.EquivalentTo(Texture1DInitialData32));
    }

    [Test]
    public void TestMapReadStaging1DRgba64()
    {
        using Texture1D<Rgba64> baseTexture = Device.CreateTexture1D<Rgba64>(Format.FormatR16G16B16A16Unorm, Texture1DInitialData64);
        using StagingTexture1D<Rgba64> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        ReadOnlySpan<Rgba64> readValues = texture.MapRead();

        Assert.That(readValues.ToArray(), Is.EquivalentTo(Texture1DInitialData64));
    }

    [Test]
    public void TestReadStagingArray2DRgba32()
    {
        using Texture2D<Rgba32> baseTexture = Device.CreateTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, Texture2DWidth, Texture2DHeight, Texture2DInitialData32);
        using StagingTexture2D<Rgba32> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba32[,] readValues = new Rgba32[Texture2DWidth, Texture2DHeight];

        texture.Read(readValues);

        Assert.That(readValues.AsSpan().ToArray(), Is.EquivalentTo(Texture2DInitialData32));
    }

    [Test]
    public void TestReadStagingArray2DRgba64()
    {
        using Texture2D<Rgba64> baseTexture = Device.CreateTexture2D<Rgba64>(Format.FormatR16G16B16A16Unorm, Texture2DWidth, Texture2DHeight, Texture2DInitialData64);
        using StagingTexture2D<Rgba64> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba64[,] readValues = new Rgba64[Texture2DWidth, Texture2DHeight];

        texture.Read(readValues);

        Assert.That(readValues.AsSpan().ToArray(), Is.EquivalentTo(Texture2DInitialData64));
    }

    [Test]
    public void TestReadStagingNewArray2DRgba32()
    {
        using Texture2D<Rgba32> baseTexture = Device.CreateTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, Texture2DWidth, Texture2DHeight, Texture2DInitialData32);
        using StagingTexture2D<Rgba32> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba32[,] readValues = texture.Read();

        Assert.That(readValues.AsSpan().ToArray(), Is.EquivalentTo(Texture2DInitialData32));
    }

    [Test]
    public void TestReadStagingNewArray2DRgba64()
    {
        using Texture2D<Rgba64> baseTexture = Device.CreateTexture2D<Rgba64>(Format.FormatR16G16B16A16Unorm, Texture2DWidth, Texture2DHeight, Texture2DInitialData64);
        using StagingTexture2D<Rgba64> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba64[,] readValues = texture.Read();

        Assert.That(readValues.AsSpan().ToArray(), Is.EquivalentTo(Texture2DInitialData64));
    }

    [Test]
    public void TestMapReadStaging2DRgba32()
    {
        using Texture2D<Rgba32> baseTexture = Device.CreateTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, Texture2DWidth, Texture2DHeight, Texture2DInitialData32);
        using StagingTexture2D<Rgba32> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        ReadOnlySpan2D<Rgba32> readValues = texture.MapRead();

        Assert.That(readValues.ToArray(), Is.EquivalentTo(Texture2DInitialData32));
    }

    [Test]
    public void TestMapReadStaging2DRgba64()
    {
        using Texture2D<Rgba64> baseTexture = Device.CreateTexture2D<Rgba64>(Format.FormatR16G16B16A16Unorm, Texture2DWidth, Texture2DHeight, Texture2DInitialData64);
        using StagingTexture2D<Rgba64> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        ReadOnlySpan2D<Rgba64> readValues = texture.MapRead();

        Assert.That(readValues.ToArray(), Is.EquivalentTo(Texture2DInitialData64));
    }

    [Test]
    public void TestMapWrite1D()
    {
        using Texture1D<Rgba32> texture = Device.CreateTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, Texture1DInitialData32.Length, cpuWritable: true);
        using StagingTexture1D<Rgba32> stagingTexture = Device.CreateStagingTexture1D(texture);

        texture.Write(Texture1DInitialData32);

        texture.CopyTo(stagingTexture);

        Assert.That(stagingTexture.Read(), Is.EquivalentTo(Texture1DInitialData32));
    }

    [Test]
    public void TestMapWriteFails1D()
    {
        using Texture1D<Rgba32> unwritableTexture = Device.CreateTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, 256);
        using Texture1D<Rgba32> writableTexture = Device.CreateTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, 256, cpuWritable: true);

        Assert.Multiple(() =>
        {
            Assert.Throws<Exception>(() =>
            {
                unwritableTexture.MapWrite();
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                writableTexture.MapWrite();

                try
                {
                    writableTexture.MapWrite();
                }
                finally
                {
                    writableTexture.Unmap();
                }
            });

            // Should be unmapped at this point
            Assert.Throws<InvalidOperationException>(() =>
            {
                writableTexture.Unmap();
            });
        });
    }

    [Test]
    public void TestMapWrite2D()
    {
        using Texture2D<Rgba32> texture = Device.CreateTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, Texture2DWidth, Texture2DHeight, cpuWritable: true);
        using StagingTexture2D<Rgba32> stagingTexture = Device.CreateStagingTexture2D(texture);

        texture.Write(Texture2DInitialData32.AsSpan().AsSpan2D(Texture2DHeight, Texture2DWidth));

        texture.CopyTo(stagingTexture);

        Assert.That(stagingTexture.Read(), Is.EquivalentTo(Texture2DInitialData32));
    }

    [Test]
    public void TestMapWriteFails2D()
    {
        using Texture2D<Rgba32> unwritableTexture = Device.CreateTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, 32, 32);
        using Texture2D<Rgba32> writableTexture = Device.CreateTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, 32, 32, cpuWritable: true);

        Assert.Multiple(() =>
        {
            Assert.Throws<Exception>(() =>
            {
                unwritableTexture.MapWrite();
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                writableTexture.MapWrite();

                try
                {
                    writableTexture.MapWrite();
                }
                finally
                {
                    writableTexture.Unmap();
                }
            });

            // Should be unmapped at this point
            Assert.Throws<InvalidOperationException>(() =>
            {
                writableTexture.Unmap();
            });
        });
    }
}
