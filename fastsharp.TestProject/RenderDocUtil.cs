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
    private struct RENDERDOC_API_1_1_2
    {
        nint GetAPIVersion;

        nint SetCaptureOptionU32;

        nint SetCaptureOptionF32;

        nint GetCaptureOptionU32;

        nint GetCaptureOptionF32;

        nint SetFocusToggleKeys;

        nint SetCaptureKeys;

        nint GetOverlayBits;

        nint MaskOverlayBits;

        nint RemoveHooks;

        nint UnloadCrashHandler;

        nint SetCaptureFilePathTemplate;

        nint GetCaptureFilePathTemplate;

        nint GetNumCaptures;
        nint GetCapture;

        nint TriggerCapture;

        nint IsTargetControlConnected;

        nint LaunchReplayUI;

        nint SetActiveWindow;

        public delegate* unmanaged[Cdecl]<nint, nint, void> StartFrameCapture;

        public delegate* unmanaged[Cdecl]<uint> IsFrameCapturing;

        public delegate* unmanaged[Cdecl]<nint, nint, void> EndFrameCapture;

        nint TriggerMultiFrameCapture;

        nint SetCaptureFileComments;

        nint DiscardFrameCapture;

        nint ShowReplayUI;

        nint SetCaptureTitle;
    }
}
