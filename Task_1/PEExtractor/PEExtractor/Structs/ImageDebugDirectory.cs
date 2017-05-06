using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ImageDebugDirectory
    {

        [FieldOffset(0)]
        public UInt32 Characteristics;
        [FieldOffset(4)]
        public UInt32 TimeDateStamp;
        [FieldOffset(8)]
        public UInt16 MajorVersion;
        [FieldOffset(10)]
        public UInt16 MinorVersion;
        [FieldOffset(12)]
        public UInt32 Type;
        [FieldOffset(16)]
        public UInt32 SizeOfData;
        [FieldOffset(20)]
        public UInt32 AddressOfRawData;
        [FieldOffset(24)]
        public UInt32 PointerToRawData;
    }
}
