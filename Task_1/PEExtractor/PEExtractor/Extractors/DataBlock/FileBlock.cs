using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PEExtractor.Common.Extension;

namespace PEExtractor.Extractors.DataBlock
{
    public class FileBlock : IBlock
    {

        private bool keepOpen;
        private BinaryReader reader;

        public FileBlock(BinaryReader reader, uint size, uint rawPointer, bool keepOpen = false)
        {
            this.reader = reader;
            this.Size = size;
            this.Pointer = rawPointer;
            this.keepOpen = keepOpen;
        }


        public BinaryReader BlockReader { get { reader.SetStreamPosition(Pointer); return reader; } }

        public uint Pointer { get; private set; }

        public uint Size { get; private set; }

        public void Dispose()
        {
            if (!keepOpen)
            {
                reader.Dispose();
            }
        }
    }
}
