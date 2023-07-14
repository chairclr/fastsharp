using FastSharp.Providers;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;

namespace FastSharp;

public partial class Device : IDisposable
{
    internal ComPtr<ID3D11Device> GraphicsDevice;

    internal ComPtr<ID3D11DeviceContext> GraphicsDeviceContext;

#if DEBUG
    private Task DebugInfoQueueTask;

    private CancellationTokenSource DebugInfoQueueCancellationTokenSource;
#endif

    private bool Disposed;

    public unsafe Device()
    {
        SilkMarshal.ThrowHResult
        (
            D3D11Provider.D3D11.CreateDevice
            (
                pAdapter: default(ComPtr<IDXGIAdapter>),
                DriverType: D3DDriverType.Hardware,
                Software: 0,
#if DEBUG
                Flags: (uint)CreateDeviceFlag.Debug,
#else
                Flags: (uint)CreateDeviceFlag.None,
#endif
                pFeatureLevels: null,
                FeatureLevels: 0,
                SDKVersion: D3D11.SdkVersion,
                ppDevice: ref GraphicsDevice,
                pFeatureLevel: null,
                ppImmediateContext: ref GraphicsDeviceContext
            )
        );

#if DEBUG
        DebugInfoQueueCancellationTokenSource = new CancellationTokenSource();

        DebugInfoQueueTask = GraphicsDevice.SetInfoQueueCallback(x =>
        {
            switch (x.Severity)
            {
                case MessageSeverity.Corruption:
                case MessageSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case MessageSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case MessageSeverity.Info:
                case MessageSeverity.Message:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }

            Console.WriteLine($"[{x.Severity}] {SilkMarshal.PtrToString((nint)x.PDescription)}");

            Console.ResetColor();
        }, DebugInfoQueueCancellationTokenSource.Token);
#endif
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
#if DEBUG
            DebugInfoQueueCancellationTokenSource.Cancel();
#endif

            GraphicsDeviceContext.Dispose();

            GraphicsDevice.Dispose();

#if DEBUG
            DebugInfoQueueTask.Wait();
#endif

            Disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
