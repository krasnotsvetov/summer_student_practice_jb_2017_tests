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
    public enum HeapType
    {
        GUID,
        Strings,
        Blob
    }

    /// <summary>
    /// A columnb type which presents index to heap
    /// </summary>
    public class HeapIndex : IColumnElement
    {
        /// <summary>
        /// Heap type
        /// </summary>
        public HeapType HeapType { get; private set; }

        /// <summary>
        /// Heap element
        /// </summary>
        public UInt32 Value       { get; set; }
        //public IEither<UInt32, UInt16> Value { get; set; }

        public HeapIndex(HeapType type)
        {
            HeapType = type; 
        }

        public IColumnElement Clone()
        {
            return new HeapIndex(HeapType);
        }

        public void ReadValue(BinaryReader reader, TablesContext context)
        {
            if (context.HeapIndexSize[HeapType] == 2)
            {
                Value = reader.ReadUInt16();
            }
            else
            {
                Value = reader.ReadUInt32();
            }
        }

        public void Report(StreamWriter sw)
        {
            sw.Write(Value);
        }
    }
}
