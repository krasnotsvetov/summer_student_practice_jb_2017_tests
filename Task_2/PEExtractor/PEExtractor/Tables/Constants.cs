using PEExtractor.Common.Extension;
using PEExtractor.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Tables
{
    /// TODO, think about returning back generic class. But what should we do with reader? Moar reflection:)

    /// <summary>
    /// Presents a UInt32 column type
    /// </summary>
    public class ConstantUInt32 : IColumnElement
    {
        public UInt32 Value { get; set; }

        public IColumnElement Clone()
        {
            return new ConstantUInt32();
        }

        public void ReadValue(BinaryReader reader, TablesContext context)
        {
            Value = reader.ReadUInt32();
        }

        public void Report(StreamWriter sw)
        {
            sw.Write(Value);
        }
    }

    /// <summary>
    /// Presents a byte column type
    /// </summary>
    public class ConstantUInt16 : IColumnElement
    {
        public UInt16 Value { get; set; }

        public IColumnElement Clone()
        {
            return new ConstantUInt16();
        }

        public void ReadValue(BinaryReader reader, TablesContext context)
        {
            Value = reader.ReadUInt16();
        }

        public void Report(StreamWriter sw)
        {
            sw.Write(Value);
        }
    }

    /// <summary>
    /// Presents a UInt16 column type
    /// </summary>
    public class ConstantByte: IColumnElement
    {
        public Byte Value { get; set; }

        public IColumnElement Clone()
        {
            return new ConstantByte();
        }

        public void ReadValue(BinaryReader reader, TablesContext context)
        {
            Value = reader.ReadByte();
        }

        public void Report(StreamWriter sw)
        {
            sw.Write(Value);
        }   
    }
}
