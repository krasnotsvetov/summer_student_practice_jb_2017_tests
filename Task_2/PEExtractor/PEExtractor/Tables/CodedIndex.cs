using PEExtractor.Common;
using PEExtractor.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Tables
{
    /// <summary>
    /// CodedIndex for column type
    /// </summary>
    public partial class CodedIndex : IColumnElement
    {
        public String Tag { get; private set; }

        /// int16 and int32 is keeped in int32, it's more comfortable

        /// <summary>
        /// Name of table
        /// </summary>
        public String TableName { get; private set; }

        /// <summary>
        /// Row id of table
        /// </summary>
        public UInt32 RowID { get; private set; }

        //public IEither<UInt32, UInt16> Value;


        public CodedIndex(string tag)
        {
            if (!Tags.ContainsKey(tag))
            {
                throw new ArgumentException("Tag is not contained in Tags");
            }
            this.Tag = tag;
        }

        public IColumnElement Clone()
        {
            return new CodedIndex(Tag);
        }

        public void ReadValue(BinaryReader reader, TablesContext context)
        {
            uint maxRowCount = 0;
            var tags = Tags[Tag];
            foreach (var v in tags.Values)
            {
                int tableIndex = context.NamesToRealNum[v];
                if (context.RowCount.ContainsKey(tableIndex))
                {
                    maxRowCount = Math.Max(maxRowCount, context.RowCount[tableIndex]);
                }
            }

            int tablesBit = (int)Math.Ceiling(Math.Log(tags.Values.Count, 2));

            UInt32 Value;

            if (maxRowCount < (1 << (16 - tablesBit)))
            {
                Value = reader.ReadUInt16();
            }
            else
            {
                Value = reader.ReadUInt32();
            }
            int mask = (1 << tablesBit) - 1;
            int tableTagId = (int)Value & mask;
            RowID = Value >> tablesBit;

            if (tableTagId >= tags.Values.Count)
            {
                throw new Exception("The data is wrong. CodedIndex can't determine table");
            }

            TableName = tags[(int)tableTagId];
        }

        public void Report(StreamWriter sw)
        {
            sw.Write($"{{{TableName} , {RowID}}}");
        }
    }
}
