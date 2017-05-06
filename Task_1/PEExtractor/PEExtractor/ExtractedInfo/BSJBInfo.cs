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
    public class BSJBInfo
    {
        /// <summary>
        /// Header
        /// </summary>
        public ImageCor20Header ImageCor20Header { get; private set; }

        /// <summary>
        /// Metadata
        /// </summary>
        public BSJBMetadataRootStructure Metadata { get { return metadata; } }

        
        private BSJBMetadataRootStructure metadata;

        public BSJBInfo(ImageCor20Header header)
        {
            this.ImageCor20Header = header;
        }


        /// <summary>
        /// Extract BSJB
        /// </summary>
        /// <param name="block">block which contains BSJB</param>
        public void Extract(IBlock block)
        {
            var reader = block.BlockReader;
            reader.CheckSignature("BSJB");
            metadata = new BSJBMetadataRootStructure();
            metadata.Signature = 0x424A5342;
            metadata.MajorVersion = reader.ReadUInt16();
            metadata.MinorVersion = reader.ReadUInt16();
            metadata.Reserved = reader.ReadUInt32();
            metadata.Length = reader.ReadUInt32();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < metadata.Length; i++)
            {
                var c = reader.PeekChar();
                if (c == 0) { break; }
                reader.ReadChar();
                sb.Append((char)c);
            }

            metadata.Version = sb.ToString();
            sb.Clear();

            reader.SetStreamPosition(reader.BaseStream.Position + metadata.Length - metadata.Version.Length);
            metadata.Flags = reader.ReadUInt16();
            metadata.Streams = reader.ReadUInt16();

            metadata.StreamHeaders = new StreamHeaders[metadata.Streams];
            for (int i = 0; i < metadata.Streams; i++)
            {
                metadata.StreamHeaders[i].Offset = reader.ReadUInt32();
                metadata.StreamHeaders[i].Size = reader.ReadUInt32();
                while (reader.PeekChar() != 0)
                {
                    sb.Append(reader.ReadChar());
                }
                //skip \0
                reader.ReadChar();
                metadata.StreamHeaders[i].Name = sb.ToString();

                // + 1 because \0 is terminal
                var length = metadata.StreamHeaders[i].Name.Length + 1;
                int alligment = 4; //
                int makeOffset = length / alligment * alligment < length ? (length / alligment + 1) * alligment : length;
                reader.SetStreamPosition(reader.BaseStream.Position + makeOffset - length);
                sb.Clear();

            }
        }


        /// <summary>
        /// Report BSJB
        /// </summary>
        /// <param name="sw"></param>
        public void Report(StreamWriter sw)
        {
            sw.WriteLine("ImageCor20Header: ");
            StructReporter.Report(ImageCor20Header, sw, 1);

            sw.WriteLine();
            sw.WriteLine();

            sw.WriteLine("BSJB Metadata: ");
            StructReporter.Report(Metadata, sw, 1, new SortedSet<string>() {"StreamHeaders"});

            sw.WriteLine("\tStream headers: ");
            for (int i = 0; i < metadata.StreamHeaders.Length; i++)
            {
                StructReporter.Report(metadata.StreamHeaders[i], sw, 2);
                sw.WriteLine();
            }

            sw.WriteLine();
            sw.WriteLine();
        }
    }
}
