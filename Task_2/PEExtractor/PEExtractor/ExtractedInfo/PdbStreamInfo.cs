using PEExtractor.Common;
using PEExtractor.Common.Extension;
using PEExtractor.Extractors.DataBlock;
using PEExtractor.Structs;
using PEExtractor.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Info
{
    public class PdbStreamInfo : IExtractable
    {
        /// <summary>
        /// Root structure of #Pdb stream info
        /// </summary>
        public PdbStreamRoot StreamRoot   { get { return streamRoot; } }

        /// <summary>
        /// Information about type system tables
        /// </summary>
        public TablesContext Context { get; private set; }

        private PdbStreamRoot streamRoot;

        public PdbStreamInfo()
        {

        }
        
        /// <summary>
        /// Extreat root structure
        /// </summary>
        /// <param name="block"></param>
        public void Extract(IBlock block)
        {
            var reader = block.BlockReader;
            streamRoot.Id = reader.ReadBytes(20);
            streamRoot.EntryPoint = reader.ReadUInt32();
            streamRoot.ReferencedTypeSystemTables = reader.ReadUInt64();

            Context = new TablesContext(streamRoot.ReferencedTypeSystemTables, null);

            streamRoot.TypeSystemTableRows = new UInt32[Context.TableCount];
            for (int i = 0; i < streamRoot.TypeSystemTableRows.Length; i++)
            {
                streamRoot.TypeSystemTableRows[i] = reader.ReadUInt32();
            }
        } 

        /// <summary>
        /// Report information
        /// </summary>
        /// <param name="sw"></param>
        public void Report(StreamWriter sw)
        {
            sw.WriteLine("Pdb stream root structure");
            sw.WriteLine("\tID: ");
            sw.WriteLine(streamRoot.Id.Select(t => (char)t).ToArray());
            sw.WriteLine($"\tEntryPoint: {streamRoot.EntryPoint}");
            sw.WriteLine($"\tReferencedTypeSystemTables: {streamRoot.ReferencedTypeSystemTables}");
        }
    } 
}
