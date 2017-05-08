using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Tables
{
    public class Row
    {
        /// <summary>
        /// Scheme for row
        /// </summary>
        public RowScheme Scheme { get; private set; }

        public Row(RowScheme scheme)
        {
            this.Scheme = scheme;
        }

        internal void Report(StreamWriter sw)
        {
            foreach (var element in Scheme.RowElements)
            {
                element.Report(sw);
                sw.Write("\t\t");
            }
        }
    }
}