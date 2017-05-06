using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Extractors.DataBlock
{
    public interface IBlock : IDisposable
    {
        BinaryReader BlockReader { get; }
        uint Pointer                 { get; }
        uint Size                    { get; }
    }
}
