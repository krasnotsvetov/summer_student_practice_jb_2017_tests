using PEExtractor.Common;
using PEExtractor.Common.Extension;
using PEExtractor.Extractors.DataBlock;
using PEExtractor.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Info
{
    public class MPDBInfo
    {
        /// <summary>
        /// Additional report information
        /// </summary>
        public string ReportInformation         { get; internal set; }

        /// <summary>
        /// Debug entry which connected with MPDB
        /// </summary>
        public ImageDebugDirectory DebugEntry   { get; private set;  } 
       

        /// <summary>
        /// Size of portable pdb data
        /// </summary>
        public uint Size                        { get; private set; }

        /// <summary>
        /// Pdb data
        /// </summary>
        public byte[] Data                      { get; private set; }

        public MPDBInfo(ImageDebugDirectory debugEntry)
        {
            this.DebugEntry = debugEntry;
        }


        /// <summary>
        /// Extract MPDB
        /// </summary>
        /// <param name="block">block which contains MPDB information</param>
        public void Extract(IBlock block)
        {
            //TODO : add lazy extract. Question: how to save "reader" correctly
            var reader = block.BlockReader;
            reader.CheckSignature("MPDB");
            Size = reader.ReadUInt32();
            
            try
            {
                Data = new byte[Size];

                using (var ds = new DeflateStream(reader.BaseStream, CompressionMode.Decompress, true))
                {
                    ds.Read(Data, 0, (int)Size);
                    if (ds.ReadByte() != -1)
                    {
                        throw new FormatException("MPDB image format is wrong. Uncompressed size is smaller than real size.");
                    }
                }
            } catch
            {
                throw new OutOfMemoryException("MPDB image out of memory");
            }

            
        }


        /// <summary>
        /// Report MPDB
        /// </summary>
        /// <param name="sw"></param>
        public void Report(StreamWriter sw)
        {
            sw.WriteLine("MPDB:");
            if (ReportInformation == null)
            {
                StructReporter.Report(DebugEntry, sw, 1);

                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine("MPDB data: ");
                sw.WriteLine($"\tSize : {Size}");
                sw.Write("\t");
              
                sw.WriteLine(Data.Select(t => (char)t).ToArray());
            }
            else
            {
                sw.WriteLine(ReportInformation);
            }

            sw.WriteLine();
            sw.WriteLine();
        }
    }
}
