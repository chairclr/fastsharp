namespace FastSharp;

public partial class Device
{
    public Texture2D CreateTexture2D(int width, int height)
    {
        return new Texture2D(this, width, height);
    }
}
