using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Installer {
    public static class NativeMethods {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
    }
}
