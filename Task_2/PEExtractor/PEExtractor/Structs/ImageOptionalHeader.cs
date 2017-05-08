using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Structs
{

    [StructLayout(LayoutKind.Explicit)]
    public struct ImageOptionalHeader32
    {
        [FieldOffset(0)]
        public UInt16 Magic;
        [FieldOffset(2)]
        public Byte MajorLinkerVersion;
        [FieldOffset(3)]
        public Byte MinorLinkerVersion;
        [FieldOffset(4)]
        public UInt32 SizeOfCode;
        [FieldOffset(8)]
        public UInt32 SizeOfInitializedData;
        [FieldOffset(12)]
        public UInt32 SizeOfUninitializedData;
        [FieldOffset(16)]
        public UInt32 AddressOfEntryPoint;
        [FieldOffset(20)]
        public UInt32 BaseOfCode;
        [FieldOffset(24)]
        public UInt32 BaseOfData;
        [FieldOffset(28)]
        public UInt32 ImageBase;
        [FieldOffset(32)]
        public UInt32 SectionAlignment;
        [FieldOffset(36)]
        public UInt32 FileAlignment;
        [FieldOffset(40)]
        public UInt16 MajorOperatingSystemVersion;
        [FieldOffset(42)]
        public UInt16 MinorOperatingSystemVersion;
        [FieldOffset(44)]
        public UInt16 MajorImageVersion;
        [FieldOffset(46)]
        public UInt16 MinorImageVersion;
        [FieldOffset(48)]
        public UInt16 MajorSubsystemVersion;
        [FieldOffset(50)]
        public UInt16 MinorSubsystemVersion;
        [FieldOffset(52)]
        public UInt32 Win32VersionValue;
        [FieldOffset(56)]
        public UInt32 SizeOfImage;
        [FieldOffset(60)]
        public UInt32 SizeOfHeaders;
        [FieldOffset(64)]
        public UInt32 CheckSum;
        [FieldOffset(68)]
        public UInt16 Subsystem;
        [FieldOffset(70)]
        public UInt16 DllCharacteristics;
        [FieldOffset(72)]
        public UInt32 SizeOfStackReserve;
        [FieldOffset(76)]
        public UInt32 SizeOfStackCommit;
        [FieldOffset(80)]
        public UInt32 SizeOfHeapReserve;
        [FieldOffset(84)]
        public UInt32 SizeOfHeapCommit;
        [FieldOffset(88)]
        public UInt32 LoaderFlags;
        [FieldOffset(92)]
        public UInt32 NumberOfRvaAndSizes;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ImageOptionalHeader64
    {
        [FieldOffset(0)]
        public UInt16 Magic;
        [FieldOffset(2)]
        public Byte MajorLinkerVersion;
        [FieldOffset(3)]
        public Byte MinorLinkerVersion;
        [FieldOffset(4)]
        public UInt32 SizeOfCode;
        [FieldOffset(8)]
        public UInt32 SizeOfInitializedData;
        [FieldOffset(12)]
        public UInt32 SizeOfUninitializedData;
        [FieldOffset(16)]
        public UInt32 AddressOfEntryPoint;
        [FieldOffset(20)]
        public UInt32 BaseOfCode;
        [FieldOffset(24)]
        public UInt64 ImageBase;
        [FieldOffset(32)]
        public UInt32 SectionAlignment;
        [FieldOffset(36)]
        public UInt32 FileAlignment;
        [FieldOffset(40)]
        public UInt16 MajorOperatingSystemVersion;
        [FieldOffset(42)]
        public UInt16 MinorOperatingSystemVersion;
        [FieldOffset(44)]
        public UInt16 MajorImageVersion;
        [FieldOffset(46)]
        public UInt16 MinorImageVersion;
        [FieldOffset(48)]
        public UInt16 MajorSubsystemVersion;
        [FieldOffset(50)]
        public UInt16 MinorSubsystemVersion;
        [FieldOffset(52)]
        public UInt32 Win32VersionValue;
        [FieldOffset(56)]
        public UInt32 SizeOfImage;
        [FieldOffset(60)]
        public UInt32 SizeOfHeaders;
        [FieldOffset(64)]
        public UInt32 CheckSum;
        [FieldOffset(68)]
        public UInt16 Subsystem;
        [FieldOffset(70)]
        public UInt16 DllCharacteristics;
        [FieldOffset(72)]
        public UInt64 SizeOfStackReserve;
        [FieldOffset(80)]
        public UInt64 SizeOfStackCommit;
        [FieldOffset(88)]
        public UInt64 SizeOfHeapReserve;
        [FieldOffset(96)]
        public UInt64 SizeOfHeapCommit;
        [FieldOffset(104)]
        public UInt32 LoaderFlags;
        [FieldOffset(108)]
        public UInt32 NumberOfRvaAndSizes;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ImageDataDirectory
    {
        [FieldOffset(0)]
        public UInt32 VirtualAddress;
        [FieldOffset(4)]
        public UInt32 Size;
    }
}
