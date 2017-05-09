using PEExtractor.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Tables
{
    public class Table
    {
        /// <summary>
        /// Table's rows
        /// </summary>
        public IReadOnlyList<Row> Rows { get { return rows.AsReadOnly(); } }

        /// <summary>
        /// Table's name
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// Table's ID
        /// </summary>
        public Int32 ID { get; private set; }

        private List<Row> rows = new List<Row>();
        private RowScheme scheme;
        private List<string> columnNames;

        /// <summary>
        /// Size of row in bytes
        /// </summary>
        private int rowPhysicalSize = 0;

        public Table(int id, TableInfo tableInfo)
        {
            this.columnNames = tableInfo.ColumnNames;
            this.Name = tableInfo.Name;
            this.scheme = new RowScheme(tableInfo.Elements);
            this.ID = id;
        }

        /// <summary>
        /// Add a row to table.
        /// </summary>
        /// <param name="row"></param>
        /*public void AddRow(Row row)
        {
            if (!scheme.IsAccept(row))
            {
                throw new ArgumentException("The row's scheme is not satisfied to table's row's scheme", nameof(row));
            }
            rows.Add(row);
        }*/
        
        /// <summary>
        /// Read row from stream and add it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context"></param>
        internal void ReadAndAddRow(BinaryReader reader, TablesContext context)
        {
            Row row = new Row(scheme.Clone());
            rowPhysicalSize = 0;
            foreach (var element in row.Scheme.RowElements)
            {
                element.ReadValue(reader, context);
                rowPhysicalSize += element.GetPhysicalSize();
            }
            rows.Add(row);
        }

        public void ReportHeader(StreamWriter sw)
        {
            if (rows.Count > 0) {
                //sw.WriteLine(string.Format("{0, -10}\t{1, -10}", ID, Name));
                sw.WriteLine($"|{$"{"0x"+ $"{ID:X2}"}", -10}\t|\t{Name, -25}\t|\t{rowPhysicalSize, -15}\t|\t{rows.Count, -10}|");
            }
        }

        public void Report(StreamWriter sw, StringsHeapInfo heapInfo, GUIDHeapInfo guidInfo)
        {

            if (rows.Count > 0)
            {
                sw.WriteLine($"------------------------------------------ {Name} ----------------------------------------");

                sw.WriteLine();
                sw.WriteLine($"|{"RowPhysicalSize",-20}\t|\t{"Row count",-20}\t|\t{"Table name",-20}|");
                sw.WriteLine($"|{rowPhysicalSize,-20}\t|\t{rows.Count,-20}\t|\t{Name, -20}|");
                sw.WriteLine();

                sw.WriteLine($"|{"Element name",-20}\t|\t{"Element offset",-20}\t|\t{"Element size (in bytes)",-20}\t|\t{"Element type",-30}|");
                int curOffset = 0;
                for (int i = 0; i < rows[0].Scheme.RowElements.Count; i++)
                {
                    var element = rows[0].Scheme.RowElements[i];

                    string elementType = "";
                    switch (element)
                    {
                        case ConstantByte b:
                            elementType = "Constant";
                            break;
                        case ConstantUInt16 i16:
                            elementType = "Constant";
                            break;
                        case ConstantUInt32 i32:
                            elementType = "Constant";
                            break;
                        case HeapIndex hi:
                            elementType = "Heap index to " + hi.HeapType.ToString();
                            break;
                        case CodedIndex ci:
                            elementType = "Coded index " + ci.Tag;
                            break;
                        case TableIndex ti:
                            elementType = "Table index " + ti.Name;
                            break;
                    }

                    sw.WriteLine($"|{columnNames[i], -20}\t|\t{$"0x{curOffset:X8}",-20}\t|\t{element.GetPhysicalSize(),-20}\t|\t{elementType,-30}|");
                    curOffset += element.GetPhysicalSize();
                }

                sw.WriteLine();


                sw.Write($"|{"№", -15}|");
                foreach (var s in columnNames)
                {
                    sw.Write($"{s,-30}|");
                }
                sw.WriteLine();
                        
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    sw.Write($"{$"0x{i + 1:X8}", -15}");
                    sw.Write("|");
                    foreach (var e in row.Scheme.RowElements)
                    {
                        string elementValue= "";
                        switch (e)
                        {
                            case ConstantByte b:
                                elementValue = $"0x{b.Value:X2}";
                                break;
                            case ConstantUInt16 i16:
                                elementValue = $"0x{i16.Value:X4}";
                                break;
                            case ConstantUInt32 i32:
                                elementValue = $"0x{i32.Value:X8}";
                                break;
                            case HeapIndex hi:
                                switch (hi.HeapType)
                                {
                                    case HeapType.Blob:
                                        if (e.GetPhysicalSize() == 2)
                                        {
                                            elementValue = $"Blob: 0x{hi.Value:X4}";
                                        } else
                                        {
                                            elementValue = $"Blob: 0x{hi.Value:X8}";
                                        }
                                        break;
                                    case HeapType.GUID:
                                        var containGUID = guidInfo.GetGuid((int)hi.Value, out var guid);
                                        if (containGUID) {
                                            elementValue = guid.ToString();
                                        } else
                                        {
                                            elementValue = "-";
                                        }
                                        break;
                                    case HeapType.Strings:
                                        elementValue = heapInfo.GetString((int)hi.Value);
                                        break;
                                }
                                break;
                            case CodedIndex ci:
                                elementValue = $"<{ci.TableName}, {ci.RowID:X8}>";
                                break;
                            case TableIndex ti:
                                if (e.GetPhysicalSize() == 2)
                                {
                                    elementValue = $"0x{ti.Value:X4}";
                                }
                                else
                                {
                                    elementValue = $"0x{ti.Value:X8}";
                                }
                                break;
                        }
                        sw.Write($"{elementValue,-30}|");
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
