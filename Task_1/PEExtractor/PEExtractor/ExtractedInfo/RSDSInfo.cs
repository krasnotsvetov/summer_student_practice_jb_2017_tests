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
    public class RSDSInfo
    {
        /// <summary>
        /// Additional report information
        /// </summary>
        public string ReportInformation         { get; internal set; }

        /// <summary>
        /// Debug entry which connected with RSDS
        /// </summary>
        public ImageDebugDirectory DebugEntry   { get; private set;  } 

        /// <summary>
        /// RSDS information
        /// </summary>
        public RSDSStruct RSDS                  { get { return rsds; } }

        public RSDSStruct rsds;


        public RSDSInfo(ImageDebugDirectory debugEntry)
        {
            this.DebugEntry = debugEntry;
        }

        /// <summary>
        /// Extract RSDS from byte block
        /// </summary>
        /// <param name="block">block which contains RSDS</param>
        public void Extract(IBlock block)
        {
            var reader = block.BlockReader;
            reader.CheckSignature("RSDS");

            rsds.Guid = new Guid(reader.ReadBytes(Marshal.SizeOf(typeof(Guid))));
            rsds.Age = reader.ReadUInt32();
            StringBuilder sb = new StringBuilder();
            while (reader.PeekChar() != 0)
            {
                sb.Append(reader.ReadChar());
            }
            rsds.PdbFilePath = sb.ToString();
        }

        /// <summary>
        /// Report RSDS
        /// </summary>
        /// <param name="sw"></param>
        public void Report(StreamWriter sw)
        {
            sw.WriteLine("RSDS:");
            if (ReportInformation == null)
            {
                sw.WriteLine("debug entry:");
                StructReporter.Report(DebugEntry, sw, 1);

                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine("RSDS: ");
                sw.WriteLine($"\tGuid : {rsds.Guid}");
                StructReporter.Report(rsds, sw, 1, new SortedSet<string>() { nameof(rsds.Guid) });
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
