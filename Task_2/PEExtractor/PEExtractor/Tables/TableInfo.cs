using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Tables
{
    public class TableInfo
    {
        /// <summary>
        /// Table name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Types of columns
        /// </summary>
        public List<IColumnElement> Elements { get; private set; }

        /// <summary>
        /// Columns name. Only for reporting
        /// </summary>
        public List<String> ColumnNames { get; private set; }

        /// <summary>
        /// Create a table infp
        /// </summary>
        /// <param name="name">Table name</param>
        /// <param name="elements">Table columns. Name and type</param>
        public TableInfo(string name, List<Tuple<string, IColumnElement>> elements)
        {
            this.Elements = elements.Select(t => t.Item2).ToList();
            this.ColumnNames = elements.Select(t => t.Item1).ToList();
            this.Name = name;
        }
    }
}
