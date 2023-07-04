using FastSharp.Providers;
using Silk.NET.Core.Native;

namespace FastSharp.Shaders;

internal class ShaderCompiler
{
    public const uint SHADER_DEBUG = (1 << 0);
    public const uint SHADER_SKIP_OPTIMIZATION = (1 << 2);

    public unsafe static T CompileFromFile<T>(Device device, string path, string entryPoint, string shaderModel)
        where T : Shader
    {
        T shader = (T)Activator.CreateInstance(typeof(T), device)!;

        Blob shaderErrors = new Blob();

        uint flags = 0;

#if DEBUG
        flags |= SHADER_DEBUG | SHADER_SKIP_OPTIMIZATION;
#endif

        string src = File.ReadAllText(path);

        nint nativeSourceString = SilkMarshal.StringToPtr(src);

        int hr = D3DCompilerProvider.D3DCompiler.Compile((void*)nativeSourceString, (nuint)src.Length, Path.GetFullPath(path), null, (ID3DInclude*)1, entryPoint, shaderModel, flags, 0, shader.ShaderData.GraphicsBlob.GetAddressOf(), shaderErrors.GraphicsBlob.GetAddressOf());

        SilkMarshal.FreeString(nativeSourceString);

        ErrorCheck(hr, shaderErrors);

        shader.Create();

        return shader;
    }

    public unsafe static T CompileFromSourceCode<T>(Device device, string shaderSource, string entryPoint, string shaderModel)
        where T : Shader
    {
        T shader = (T)Activator.CreateInstance(typeof(T), device)!;

        Blob shaderErrors = new Blob();

        uint flags = 0;

#if DEBUG
        flags |= SHADER_DEBUG | SHADER_SKIP_OPTIMIZATION;
#endif

        nint nativeSourceString = SilkMarshal.StringToPtr(shaderSource);

        int hr = D3DCompilerProvider.D3DCompiler.Compile((void*)nativeSourceString, (nuint)shaderSource.Length, (string?)null, null, null, entryPoint, shaderModel, flags, 0, shader.ShaderData.GraphicsBlob.GetAddressOf(), shaderErrors.GraphicsBlob.GetAddressOf());

        SilkMarshal.FreeString(nativeSourceString);

        ErrorCheck(hr, shaderErrors);

        shader.Create();

        return shader;
    }

    private static void ErrorCheck(int hr, Blob shaderErrors)
    {
        if (HResult.IndicatesFailure(hr))
        {
            if (!shaderErrors.IsNull)
            {
                string compilerErrors = shaderErrors.AsString()!;

                shaderErrors.Dispose();

                Console.ForegroundColor = ConsoleColor.Red;

                string[] errors = compilerErrors.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }

                Console.ResetColor();

                throw new Exception($"Failed to compile shader.\n'{compilerErrors}'");
            }
            else
            {
                SilkMarshal.ThrowHResult(hr);
            }
        }
    }
}
