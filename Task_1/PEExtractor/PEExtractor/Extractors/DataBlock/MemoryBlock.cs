using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PEExtractor.Common.Extension;

namespace PEExtractor.Extractors.DataBlock
{
    public unsafe class MemoryBlock : IBlock
    {

        private BinaryReader reader;

        public MemoryBlock(IntPtr virtualPointer, uint size)
        {
            var stream = new UnmanagedMemoryStream((byte*)virtualPointer.ToPointer(), size);

            this.reader = new BinaryReader(stream);
            this.Size = size;
        }


        public BinaryReader BlockReader { get { return reader; } }

        public uint Pointer { get; private set; }

        public uint Size { get; private set; }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
