using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;
using FastSharp.Shaders;
using Silk.NET.DXGI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FastSharp.TextureTests;

public class MapTextureTests
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
    public void TestReadStagingArray1DRgba32()
    {
        Rgba32[] values = new Rgba32[8]
        {
            Color.Red, Color.RebeccaPurple, Color.SeaGreen, Color.Blue, Color.Yellow, Color.Orange, Color.Orchid, Color.Orchid
        };

        using Texture1D<Rgba32> baseTexture = Device.CreateTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, values);
        using StagingTexture1D<Rgba32> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba32[] readValues = new Rgba32[8];

        texture.Read(readValues);

        Assert.That(readValues, Is.EquivalentTo(values));
    }

    [Test]
    public void TestReadStagingArray1DRgba64()
    {
        Rgba64[] values = new Rgba64[8]
        {
            Color.Red, Color.RebeccaPurple, Color.SeaGreen, Color.Blue, Color.Yellow, Color.Orange, Color.Orchid, Color.Orchid
        };

        using Texture1D<Rgba64> baseTexture = Device.CreateTexture1D<Rgba64>(Format.FormatR16G16B16A16Unorm, values);
        using StagingTexture1D<Rgba64> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba64[] readValues = new Rgba64[8];

        texture.Read(readValues);

        Assert.That(readValues, Is.EquivalentTo(values));
    }

    [Test]
    public void TestReadStagingNewArray1DRgba32()
    {
        Rgba32[] values = new Rgba32[8]
        {
            Color.Red, Color.RebeccaPurple, Color.SeaGreen, Color.Blue, Color.Yellow, Color.Orange, Color.Orchid, Color.Orchid
        };

        using Texture1D<Rgba32> baseTexture = Device.CreateTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, values);
        using StagingTexture1D<Rgba32> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba32[] readValues = texture.Read();

        Assert.That(readValues, Is.EquivalentTo(values));
    }

    [Test]
    public void TestReadStagingNewArray1DRgba64()
    {
        Rgba64[] values = new Rgba64[8]
        {
            Color.Red, Color.RebeccaPurple, Color.SeaGreen, Color.Blue, Color.Yellow, Color.Orange, Color.Orchid, Color.Orchid
        };

        using Texture1D<Rgba64> baseTexture = Device.CreateTexture1D<Rgba64>(Format.FormatR16G16B16A16Unorm, values);
        using StagingTexture1D<Rgba64> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba64[] readValues = texture.Read();

        Assert.That(readValues, Is.EquivalentTo(values));
    }

    [Test]
    public void TestMapReadStaging1DRgba32()
    {
        Rgba32[] values = new Rgba32[8]
        {
            Color.Red, Color.RebeccaPurple, Color.SeaGreen, Color.Blue, Color.Yellow, Color.Orange, Color.Orchid, Color.Orchid
        };

        using Texture1D<Rgba32> baseTexture = Device.CreateTexture1D<Rgba32>(Format.FormatR8G8B8A8Unorm, values);
        using StagingTexture1D<Rgba32> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        ReadOnlySpan<Rgba32> readValues = texture.MapRead();

        Assert.That(readValues.ToArray(), Is.EquivalentTo(values));
    }

    [Test]
    public void TestMapReadStaging1DRgba64()
    {
        Rgba64[] values = new Rgba64[8]
        {
            Color.Red, Color.RebeccaPurple, Color.SeaGreen, Color.Blue, Color.Yellow, Color.Orange, Color.Orchid, Color.Orchid
        };

        using Texture1D<Rgba64> baseTexture = Device.CreateTexture1D<Rgba64>(Format.FormatR16G16B16A16Unorm, values);
        using StagingTexture1D<Rgba64> texture = Device.CreateStagingTexture1D(baseTexture);

        baseTexture.CopyTo(texture);

        ReadOnlySpan<Rgba64> readValues = texture.MapRead();

        Assert.That(readValues.ToArray(), Is.EquivalentTo(values));
    }

    [Test]
    public void TestReadStagingArray2DRgba32()
    {
        Rgba32[] values = new Rgba32[4]
        {
            Color.Red, Color.RebeccaPurple,
            Color.SeaGreen, Color.Blue
        };

        using Texture2D<Rgba32> baseTexture = Device.CreateTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 2, values);
        using StagingTexture2D<Rgba32> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba32[,] readValues = new Rgba32[2,2];

        texture.Read(readValues);

        Assert.That(readValues.AsSpan().ToArray(), Is.EquivalentTo(values));
    }

    [Test]
    public void TestReadStagingArray2DRgba64()
    {
        Rgba64[] values = new Rgba64[4]
        {
            Color.Red, Color.RebeccaPurple,
            Color.SeaGreen, Color.Blue
        };

        using Texture2D<Rgba64> baseTexture = Device.CreateTexture2D<Rgba64>(Format.FormatR16G16B16A16Unorm, 2, 2, values);
        using StagingTexture2D<Rgba64> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba64[,] readValues = new Rgba64[2, 2];

        texture.Read(readValues);

        Assert.That(readValues.AsSpan().ToArray(), Is.EquivalentTo(values));
    }

    [Test]
    public void TestReadStagingNewArray2DRgba32()
    {
        Rgba32[] values = new Rgba32[4]
        {
            Color.Red, Color.RebeccaPurple,
            Color.SeaGreen, Color.Blue
        };

        using Texture2D<Rgba32> baseTexture = Device.CreateTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm,2, 2, values);
        using StagingTexture2D<Rgba32> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba32[,] readValues = texture.Read();

        Assert.That(readValues.AsSpan().ToArray(), Is.EquivalentTo(values));
    }

    [Test]
    public void TestReadStagingNewArray2DRgba64()
    {
        Rgba64[] values = new Rgba64[4]
        {
            Color.Red, Color.RebeccaPurple, Color.SeaGreen, Color.Blue
        };

        using Texture2D<Rgba64> baseTexture = Device.CreateTexture2D<Rgba64>(Format.FormatR16G16B16A16Unorm, 2, 2, values);
        using StagingTexture2D<Rgba64> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        Rgba64[,] readValues = texture.Read();

        Assert.That(readValues.AsSpan().ToArray(), Is.EquivalentTo(values));
    }

    [Test]
    public void TestMapReadStaging2DRgba32()
    {
        Rgba32[] values = new Rgba32[6]
        {
            Color.Red, Color.RebeccaPurple,
            Color.SeaGreen, Color.Blue,
            Color.Orange, Color.Green
        };

        using Texture2D<Rgba32> baseTexture = Device.CreateTexture2D<Rgba32>(Format.FormatR8G8B8A8Unorm, 2, 3, values);
        using StagingTexture2D<Rgba32> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        ReadOnlySpan2D<Rgba32> readValues = texture.MapRead();

        Assert.That(readValues.ToArray().AsSpan().ToArray(), Is.EquivalentTo(values));
    }

    [Test]
    public void TestMapReadStaging2DRgba64()
    {
        Rgba64[] values = new Rgba64[6]
        {
            Color.Red, Color.RebeccaPurple,
            Color.SeaGreen, Color.Blue,
            Color.Orange, Color.Green
        };

        using Texture2D<Rgba64> baseTexture = Device.CreateTexture2D<Rgba64>(Format.FormatR16G16B16A16Unorm, 2, 3, values);
        using StagingTexture2D<Rgba64> texture = Device.CreateStagingTexture2D(baseTexture);

        baseTexture.CopyTo(texture);

        ReadOnlySpan2D<Rgba64> readValues = texture.MapRead();

        Assert.That(readValues.ToArray().AsSpan().ToArray(), Is.EquivalentTo(values));
    }
}
