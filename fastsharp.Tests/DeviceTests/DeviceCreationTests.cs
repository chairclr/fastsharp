namespace FastSharp.DeviceTests;

public class DeviceCreationTests
{
    [Test]
    public void CreateDevice()
    {
        using (Device device = new Device())
        {

        }
        
        Assert.Pass();
    }
}