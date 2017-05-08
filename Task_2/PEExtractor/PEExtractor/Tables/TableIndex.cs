using PEExtractor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEExtractor.Info;
using System.IO;

namespace PEExtractor.Tables
{
    /// <summary>
    /// A column type which links to a row in a table which name is Name
    /// </summary>
    public class TableIndex : IColumnElement
    {
        /// <summary>
        /// Table name which is indexed
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Value
        /// </summary>
        public uint Value { get; private set; }

        ///int32 value is more comfortable for using than either. 
        ///public IEither<UInt32, UInt16> Value;
    
        private int physicalSize;

        public TableIndex(string name)
        {
            this.Name = name;
        }

        public IColumnElement Clone()
        {
            return new TableIndex(Name);
        }

        public void ReadValue(BinaryReader reader, TablesContext context)
        {
            uint rowCount = 0;
            if (context.RowCount.ContainsKey(context.NamesToRealNum[Name]))
            {
                rowCount = context.RowCount[context.NamesToRealNum[Name]];
            }
            if (rowCount < (1 << 16))
            {
                Value = reader.ReadUInt16();
                physicalSize = 2;
            } else
            {
                Value = reader.ReadUInt32();
                physicalSize = 4;
            }
        }

        public void Report(StreamWriter sw)
        {
            sw.Write(Value);
        }

        public int GetPhysicalSize()
        {
            return physicalSize;
        }
    }
}
