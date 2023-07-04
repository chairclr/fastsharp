namespace FastSharp.Shaders;

public class ShaderProfile
{
    public static readonly ShaderProfile CS5_0 = new ShaderProfile("cs_5_0");

    public static readonly ShaderProfile PS5_0 = new ShaderProfile("ps_5_0");

    public static readonly ShaderProfile VS5_0 = new ShaderProfile("vs_5_0");


    public static readonly ShaderProfile CS6_0 = new ShaderProfile("cs_6_0");

    public static readonly ShaderProfile PS6_0 = new ShaderProfile("ps_6_0");

    public static readonly ShaderProfile VS6_0 = new ShaderProfile("vs_6_0");


    private readonly string Profile;

    private ShaderProfile(string profile)
    {
        Profile = profile;
    }

    public override string ToString()
    {
        return Profile;
    }
}
