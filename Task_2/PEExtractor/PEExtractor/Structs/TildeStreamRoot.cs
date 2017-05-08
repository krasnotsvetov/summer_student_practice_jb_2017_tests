using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Structs
{
    public struct TildeStreamRoot
    {
        public byte MajorVersion;
        public byte MinorVersion;
        public byte HeapSizes;
        public UInt64 Valid;
        public UInt64 Sorted;
        public UInt32[] Rows;
    }
}
