using PEExtractor.Common;
using PEExtractor.Common.Extension;
using PEExtractor.Extractors.DataBlock;
using PEExtractor.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Info
{
    public class GUIDHeapInfo : IExtractable
    {
        private Dictionary<int, Guid> Guids = new Dictionary<int, Guid>();
        public GUIDHeapInfo()
        {
        }

        /// <summary>
        /// Extract guids
        /// </summary>
        /// <param name="block"></param>
        public void Extract(IBlock block)
        {
            var reader = block.BlockReader;
            int guidSize = Marshal.SizeOf(typeof(Guid));
            for (int i = 0; i < block.Size / guidSize; i++)
            {
                Guids[i + 1] = new Guid(reader.ReadBytes(guidSize));
            }
        }

        /// <summary>
        /// Check, that GUID with this index exists
        /// </summary>
        /// <param name="index"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool GetGuid(int index, out Guid guid)
        {
            if (Guids.ContainsKey(index))
            {
                guid = Guids[index];
                return true;
            }
            guid = Guid.Empty;
            return false;
        }

    }
}
