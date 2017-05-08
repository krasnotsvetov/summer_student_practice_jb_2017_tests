using PEExtractor.Extractors.DataBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Info
{
    public interface IExtractable
    {
        void Extract(IBlock block);
    }
}
