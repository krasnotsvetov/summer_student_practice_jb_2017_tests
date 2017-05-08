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
        /// Table name
        /// </summary>
        public string Name { get; private set; }

        private List<Row> rows = new List<Row>();
        private RowScheme scheme;
        private List<string> columnNames;


        public Table(string name, List<string> columnNames, RowScheme scheme)
        {
            this.columnNames = columnNames;
            this.Name = name;
            this.scheme = scheme;
        }

        /// <summary>
        /// Add a row to table.
        /// </summary>
        /// <param name="row"></param>
        public void AddRow(Row row)
        {
            if (!scheme.IsAccept(row))
            {
                throw new ArgumentException("The row's scheme is not satisfied to table's row's scheme", nameof(row));
            }
            rows.Add(row);
        }
        
        /// <summary>
        /// Read row from stream and add it
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context"></param>
        internal void ReadAndAddRow(BinaryReader reader, TablesContext context)
        {
            Row row = new Row(scheme.Clone());
            foreach (var element in row.Scheme.RowElements)
            {
                element.ReadValue(reader, context);
            }
            rows.Add(row);
        }

        public void Report(StreamWriter sw)
        {
            sw.WriteLine($"Table: {Name}");
            foreach (var columnName in columnNames)
            {
                sw.Write($"{columnName}\t");
            }
            sw.WriteLine();

            foreach (var row in rows)
            {
                row.Report(sw);
                sw.WriteLine();
            }
        }
    }
}
