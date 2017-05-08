using PEExtractor.Common;
using PEExtractor.Common.Extension;
using PEExtractor.Extractors.DataBlock;
using PEExtractor.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Info
{
    public class StringsHeapInfo : IExtractable
    {
        private Dictionary<int, String> Strings = new Dictionary<int, string>();
        public StringsHeapInfo()
        {
        }

        /// <summary>
        /// Extract strings
        /// </summary>
        /// <param name="block"></param>
        public void Extract(IBlock block)
        {
            var reader = block.BlockReader;
            StringBuilder sb = new StringBuilder();
            int count = 0;
            int prevIndex = 0;
            while (block.Size != count)
            {
                if (reader.PeekChar() != 0)
                {
                    sb.Append((char)reader.PeekChar());
                }
                else
                {
                    Strings[prevIndex] = sb.ToString();
                    prevIndex = count + 1;
                    sb.Clear();
                }
                reader.ReadChar();
                count++;
            }
        }

        /// <summary>
        /// Return strings if it exist or "-"
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetString(int index)
        {
            if (!Strings.ContainsKey(index))
            {
                return "-";
            }
            return Strings[index];
        }
    }
}
