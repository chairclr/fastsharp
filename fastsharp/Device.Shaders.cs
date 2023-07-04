using FastSharp.Shaders;

namespace FastSharp;

public partial class Device
{
    public T CompileShaderFromFile<T>(string filePath, string entryPoint, ShaderProfile shaderModel)
        where T : Shader
    {
        if (!File.Exists(filePath))
        {
            filePath = Path.GetFullPath(filePath);
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("cannot find shader file", filePath);
        }

        return ShaderCompiler.CompileFromFile<T>(this, filePath, entryPoint, shaderModel.ToString());
    }

    public T CompileShaderFromSource<T>(string shaderSource, string entryPoint, ShaderProfile shaderModel)
        where T : Shader
    {
        return ShaderCompiler.CompileFromSourceCode<T>(this, shaderSource, entryPoint, shaderModel.ToString());
    }
}
