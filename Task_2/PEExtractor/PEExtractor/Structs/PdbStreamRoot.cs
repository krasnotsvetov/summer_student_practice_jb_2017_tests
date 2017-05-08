using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Structs
{
    public struct PdbStreamRoot
    {
        public byte[] Id;
        public UInt32 EntryPoint;
        public UInt64 ReferencedTypeSystemTables;
        public UInt32[] TypeSystemTableRows;
    }
}
