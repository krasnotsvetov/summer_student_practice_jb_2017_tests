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

        /// <summary>
        /// Table id at tags
        /// </summary>
        public Int32 TableTagID { get; private set; }

        //public IEither<UInt32, UInt16> Value;

        /// <summary>
        /// Size in bytes
        /// </summary>
        private int physicalSize;

       

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
                physicalSize = 2;
            }
            else
            {
                Value = reader.ReadUInt32();
                physicalSize = 4;
            }
            int mask = (1 << tablesBit) - 1;
            TableTagID = (int)Value & mask;
            RowID = Value >> tablesBit;

            if (TableTagID >= tags.Values.Count)
            {
                /// https://github.com/ww898/summer_student_practice_jb_2017_tests/issues/1 
                /// It's not wrong, it can happend in real life:)

                ///throw new Exception("The data is wrong. CodedIndex can't determine table");
            }

            if (tags.ContainsKey(TableTagID)) {
                TableName = tags[TableTagID];
            } else
            {
                TableName = "Undefined, because uncoded value more than count of tables in tags";
            }
        }

        public void Report(StreamWriter sw)
        {
            sw.Write($"{{{TableName} , {RowID}}}");
        }

        public int GetPhysicalSize()
        {
            return physicalSize;
        }
    }
}
