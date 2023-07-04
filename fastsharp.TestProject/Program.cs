namespace FastSharp.TestProject;

internal class Program
{
    static void Main(string[] args)
    {
        using Device device = new Device();

        using Texture2D texture = device.CreateTexture2D(1024, 1024);
    }
}
