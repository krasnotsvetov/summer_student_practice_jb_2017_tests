using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ImageCor20Header
    {

        [FieldOffset(0)]
        public UInt32 CB;
        [FieldOffset(4)]
        public UInt16 MajorRuntimeVersion;
        [FieldOffset(6)] 
        public UInt16 MinorRuntimeVersion;
        [FieldOffset(8)]
        public ImageDataDirectory MetaData;
        [FieldOffset(16)]
        public UInt32 Flags;
        [FieldOffset(20)]
        public UInt32 EntryPointToken;
        [FieldOffset(24)]
        public ImageDataDirectory Resources;
        [FieldOffset(32)]
        public ImageDataDirectory StrongNameSignature;
        [FieldOffset(40)]
        public ImageDataDirectory CodeManagerTable;
        [FieldOffset(48)]
        public ImageDataDirectory VTableFixups;
        [FieldOffset(56)]
        public ImageDataDirectory ExportAddressTableJumps;
        [FieldOffset(64)]
        public ImageDataDirectory ManagedNativeHeader;
    }
}
