using System;
using System.Runtime.InteropServices;
using osuTK.Graphics.OpenGL4;

namespace GLPT
{
    public class DebugMessageHandler
    {
        private static DebugProc _debugProcCallback = DebugCallback;
        private static GCHandle _debugProcCallbackHandle;
        
        public static void Initialize()
        {
            _debugProcCallbackHandle = GCHandle.Alloc(_debugProcCallback);

            GL.DebugMessageCallback(_debugProcCallback, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
        }
        
        private static void DebugCallback(DebugSource source,
            DebugType type,
            int id,
            DebugSeverity severity,
            int length,
            IntPtr message,
            IntPtr userParam)
        {
            string messageString = Marshal.PtrToStringAnsi(message, length);

            Console.WriteLine($"{severity} {type} | {messageString}");

            if (type == DebugType.DebugTypeError)
            {
                throw new Exception(messageString);
            }
        }
    }
}