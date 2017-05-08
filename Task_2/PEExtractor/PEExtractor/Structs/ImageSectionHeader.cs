using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct ImageSectionHeader
    {
        //IMAGE_SIZEOF_SHORT_NAME = 8
        [FieldOffset(0)]
        public fixed Byte Name[8];

        //union
        [FieldOffset(8)]
        public UInt32 PhysicalAddress;
        [FieldOffset(8)]
        public UInt32 VirtualSize;
        [FieldOffset(12)]
        public UInt32 VirtualAddress;
        [FieldOffset(16)]
        public UInt32 SizeOfRawData;
        [FieldOffset(20)]
        public UInt32 PointerToRawData;
        [FieldOffset(24)]
        public UInt32 PointerToRelocations;
        [FieldOffset(28)]
        public UInt32 PointerToLinenumbers;
        [FieldOffset(32)]
        public UInt16 NumberOfRelocations;
        [FieldOffset(34)]
        public UInt16 NumberOfLinenumbers;
        [FieldOffset(36)]
        public UInt32 Characteristics;
    }
}
