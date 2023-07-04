using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;

namespace FastSharp.Shaders;


public abstract class Shader
{
    protected readonly Device Device;

    internal Blob ShaderData = new Blob();

    public Shader(Device device)
    {
        Device = device;
    }

    internal abstract void Create();

    internal abstract void Bind();
}

public abstract class Shader<T> : Shader, IDisposable where T : unmanaged, IComVtbl<T>, IComVtbl<ID3D11DeviceChild>
{
    internal ComPtr<T> GraphicsShader = default;

    protected Shader(Device device) : base(device)
    {

    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            ShaderData?.Dispose();

            GraphicsShader.Dispose();
        }
    }
}