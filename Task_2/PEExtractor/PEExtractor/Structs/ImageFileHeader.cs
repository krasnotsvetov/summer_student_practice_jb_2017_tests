using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ImageFileHeader
    {
        [FieldOffset(0)]
        public UInt16 Machine;
        [FieldOffset(2)]
        public UInt16 NumberOfSections;
        [FieldOffset(4)]
        public UInt32 TimeDateStamp;
        [FieldOffset(8)]
        public UInt32 PointerToSymbolTable;
        [FieldOffset(12)]
        public UInt32 NumberOfSymbols;
        [FieldOffset(16)]
        public UInt16 SizeOfOptionalHeader;
        [FieldOffset(18)]
        public UInt16 Characteristics;
    }
}
