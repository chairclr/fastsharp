using System.Runtime.InteropServices;

namespace FastSharp;

// Utility class to tell RenderDoc to capture frames
// Only used for debugging purposes
// And yes I know this code is sloppy and bad
internal unsafe static partial class RenderDocUtil
{
    public static readonly bool RenderDocEnabled = false;

    private static readonly RENDERDOC_API_1_1_2* API;

    static RenderDocUtil()
    {
        nint hmodule = GetModuleHandle("renderdoc.dll");

        if (hmodule != 0)
        {
            RenderDocEnabled = true;

            delegate* unmanaged[Cdecl]<int, RENDERDOC_API_1_1_2**, int> getapi = (delegate* unmanaged[Cdecl]<int, RENDERDOC_API_1_1_2**, int>)(GetProcAddress(hmodule, "RENDERDOC_GetAPI"));

            fixed (RENDERDOC_API_1_1_2** ptr = &API)
            {
                int ret = getapi(10102, ptr);

                if (ret != 1)
                {
                    throw new Exception("Failed to get RenderDoc api");
                }
            }
        }
    }

    public static void StartFrameCapture()
    {
        if (RenderDocEnabled)
        {
            API->StartFrameCapture(0, 0);
        }
    }

    public static void EndFrameCapture()
    {
        if (RenderDocEnabled)
        {
            API->EndFrameCapture(0, 0);
        }
    }

    [LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    private static partial nint GetModuleHandle(string module);

    [LibraryImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
    private static partial nint GetProcAddress(nint hModule, string procName);

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct RENDERDOC_API_1_1_2
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
