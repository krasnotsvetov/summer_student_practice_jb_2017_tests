using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Common
{
    public sealed class LibraryHandle : IDisposable
    {

        /// <summary>
        /// ImageBase of loaded library
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Load and execute library  with flags via LoadLibraryEx from Win32 API.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="flags"></param>
        public LibraryHandle(string path, uint flags = 0)
        {
            if (!File.Exists(path))
            {
                throw new IOException($"The file is not found: {path}");
            }
            Handle = LoadLibraryEx(path, IntPtr.Zero, flags);
        }

        /// <summary>
        /// Free library
        /// </summary>
        public void Dispose()
        {
            if (!Handle.Equals(IntPtr.Zero))
            {
                FreeLibrary(Handle);
                Handle = IntPtr.Zero;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr FreeLibrary(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
    }
}
