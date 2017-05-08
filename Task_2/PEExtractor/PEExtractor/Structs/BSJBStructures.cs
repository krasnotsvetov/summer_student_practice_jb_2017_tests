using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


namespace PEExtractor.Structs
{
    public struct BSJBMetadataRootStructure
    {
        public UInt32 Signature;
        public UInt16 MajorVersion;
        public UInt16 MinorVersion;
        public UInt32 Reserved;
        public UInt32 Length;
        public string Version;
        public UInt16 Flags;
        public UInt16 Streams;
        public StreamHeaders[] StreamHeaders;
    }

    public struct StreamHeaders
    {
        public UInt32 Offset;
        public UInt32 Size;
        public string Name;
    }
}
