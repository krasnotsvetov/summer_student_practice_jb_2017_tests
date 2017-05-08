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
    public partial class TildeStreamInfo
    {
        /// <summary>
        /// Root structure of #~ stream
        /// </summary>
        public TildeStreamRoot StreamRoot   { get { return streamRoot; } }

        /// <summary>
        /// Tables, that presented in #~ stream
        /// </summary>
        public IReadOnlyDictionary<int, Table> Tables  { get { return tables; } }

        private Dictionary<int, Table> tables = new Dictionary<int, Table>();
        private TildeStreamRoot streamRoot;
        private TablesContext context;

        private PdbStreamInfo pdbStreamInfo;

        /// <summary>
        /// pdbStreamInfo is used for extracting information if #pdb stream exists
        /// </summary>
        /// <param name="pdbStreamInfo"></param>
        public TildeStreamInfo(PdbStreamInfo pdbStreamInfo)
        {
            this.pdbStreamInfo = pdbStreamInfo;
        }

        /// <summary>
        /// Extract #~ stream root and all tables
        /// </summary>
        /// <param name="block"></param>
        public void Extract(IBlock block)
        {
            ///root
            var reader = block.BlockReader;
            int r0 = reader.ReadInt32();
            streamRoot.MajorVersion = reader.ReadByte();
            streamRoot.MinorVersion = reader.ReadByte();
            streamRoot.HeapSizes = reader.ReadByte();
            byte r1 = reader.ReadByte();
            streamRoot.Valid = reader.ReadUInt64();
            streamRoot.Sorted = reader.ReadUInt64();
            ///end root

            ///rows
            context = new TablesContext(streamRoot.Valid, pdbStreamInfo);
            context.InitializeHeapIndexSize(streamRoot);

            streamRoot.Rows = new uint[context.TableCount];
            int offset = 0;
            //merge with pfb
            if (pdbStreamInfo != null)
            {
                offset = pdbStreamInfo.Context.TableCount;
                for (int i = 0; i < offset; i++)
                {
                    //respective prb rows count
                    streamRoot.Rows[i] = pdbStreamInfo.StreamRoot.TypeSystemTableRows[i];
                }
            }
            for (int i = offset; i < context.TableCount; i++)
            {
                streamRoot.Rows[i] = reader.ReadUInt32();
            }
            context.InitializeRowCount(streamRoot.Rows);
            ///end rows

            ///map names to int
            foreach (var tableInfo in TablesInfo)
            {
                context.NamesToRealNum[tableInfo.Value.Name] = tableInfo.Key;
            }
            //Start extract tables
            foreach (var tableInfo in TablesInfo)
            {
                //check that table is presented
                if (!context.RowCount.ContainsKey(tableInfo.Key)) continue;

                //if #Pdb is include, #~ contains only debugging information tables.  Skip all type system metadata tables;
                if (pdbStreamInfo != null && tableInfo.Key < 0x30) continue;

                var newTable = new Table(tableInfo.Value.Name,tableInfo.Value.ColumnNames, new RowScheme(tableInfo.Value.Elements));
                tables[tableInfo.Key] = newTable;
                for (int i = 0; i < context.RowCount[tableInfo.Key]; i++)
                {
                    newTable.ReadAndAddRow(reader, context);
                }
            }
        }

        /// <summary>
        /// Report information
        /// </summary>
        /// <param name="sw"></param>
        public void Report(StreamWriter sw)
        {
            sw.WriteLine("#~ stream root structure");
            sw.WriteLine($"MinorVersion: {streamRoot.MinorVersion}");
            sw.WriteLine($"MajorVersion: {streamRoot.MajorVersion}");
            sw.WriteLine($"Valid: {streamRoot.Valid}");
            sw.WriteLine($"Sorted   : {streamRoot.Sorted}");

            foreach (var kvp in context.HeapIndexSize)
            {
                sw.WriteLine($"{kvp.Key.ToString()} : {kvp.Value} bytes index");
            }

            sw.WriteLine($"Rows: ");
            foreach (var r in streamRoot.Rows)
            {
                sw.Write($"{r}\t");
            }
            sw.WriteLine();
        }
    } 
}
