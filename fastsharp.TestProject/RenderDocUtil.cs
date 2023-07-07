using System.Runtime.InteropServices;
using unsafe RenderDocGetAPI = delegate* unmanaged[Cdecl]<int, ref FastSharp.RenderDocUtil.RenderDocAPI*, int>;

namespace FastSharp;

// Utility class to tell RenderDoc to capture frames
// Only used for debugging purposes
internal unsafe static partial class RenderDocUtil
{
    public static readonly bool RenderDocEnabled = false;

    private static readonly RenderDocAPI* API;

    private const int RenderDocAPIVersion = 10600;

    static RenderDocUtil()
    {
        if (NativeLibrary.TryLoad("renderdoc.dll", out nint renderdocLibrary))
        {
            Console.WriteLine($"RenderDoc Loaded");

            RenderDocEnabled = true;

            if (!NativeLibrary.TryGetExport(renderdocLibrary, "RENDERDOC_GetAPI", out nint getAPIAddress))
            {
                throw new Exception("Failed to find 'RENDERDOC_GetAPI' export");
            }

            RenderDocGetAPI getapi = (RenderDocGetAPI)getAPIAddress;

            int ret = getapi(RenderDocAPIVersion, ref API);

            if (ret != 1)
            {
                throw new Exception("Failed to get RenderDoc api");
            }
        }
    }

    public static void StartFrameCapture()
    {
        if (RenderDocEnabled)
        {
            Console.WriteLine("Starting Frame Capture");
            API->StartFrameCapture(0, 0);
        }
    }

    public static void EndFrameCapture()
    {
        if (RenderDocEnabled)
        {
            Console.WriteLine("Ending Frame Capture");
            API->EndFrameCapture(0, 0);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct RenderDocAPI
    {
        private readonly nint GetAPIVersion;

        private readonly nint SetCaptureOptionU32;

        private readonly nint SetCaptureOptionF32;

        private readonly nint GetCaptureOptionU32;

        private readonly nint GetCaptureOptionF32;

        private readonly nint SetFocusToggleKeys;

        private readonly nint SetCaptureKeys;

        private readonly nint GetOverlayBits;

        private readonly nint MaskOverlayBits;

        private readonly nint RemoveHooks;

        private readonly nint UnloadCrashHandler;

        private readonly nint SetCaptureFilePathTemplate;

        private readonly nint GetCaptureFilePathTemplate;

        private readonly nint GetNumCaptures;

        private readonly nint GetCapture;

        private readonly nint TriggerCapture;

        private readonly nint IsTargetControlConnected;

        private readonly nint LaunchReplayUI;

        private readonly nint SetActiveWindow;

        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> StartFrameCapture;

        public readonly delegate* unmanaged[Cdecl]<uint> IsFrameCapturing;

        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> EndFrameCapture;

        private readonly nint TriggerMultiFrameCapture;

        private readonly nint SetCaptureFileComments;

        private readonly nint DiscardFrameCapture;

        private readonly nint ShowReplayUI;

        private readonly nint SetCaptureTitle;
    }
}
