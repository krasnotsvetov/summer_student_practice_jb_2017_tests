using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Tables
{
    public class RowScheme
    {

        /// <summary>
        /// Column elements in row
        /// </summary>
        public IReadOnlyList<IColumnElement> RowElements { get { return rowElements; } }

        private List<IColumnElement> rowElements;

        public RowScheme(List<IColumnElement> elements)
        {
            this.rowElements = elements;
        }


        public RowScheme Clone()
        {
            var elements = new List<IColumnElement>(rowElements.Count);
            foreach (var e in rowElements)
            {
                elements.Add(e.Clone());
            }
            return new RowScheme(elements);
        }
        
        /// <summary>
        /// Check that row has same scheme
        /// </summary>
        /// <param name="row">row to check</param>
        /// <returns></returns>
        public bool IsAccept(Row row)
        {
            var testScheme = row.Scheme;
            if (testScheme.rowElements.Count != rowElements.Count)
            {
                return false;
            }

            for (int i = 0; i < rowElements.Count; i++)
            {
                var testElement = testScheme.rowElements[i];
                var curElement = rowElements[i];

                switch (curElement)
                {
                    case HeapIndex heapIndex:
                        if (testElement is HeapIndex testHeapIndex)
                        {
                            if (!heapIndex.HeapType.Equals(testHeapIndex.HeapType))
                            {
                                return false;
                            }
                        } else
                        {
                            return false;
                        }
                        break;
                    case TableIndex tableIndex:
                        if (testElement is TableIndex testTableIndex)
                        {
                            if (!tableIndex.Name.Equals(testTableIndex.Name))
                            {
                                return false;
                            }
                        } else
                        {
                            return false;
                        }
                        break;
                    case CodedIndex codeIndex:
                        if (testElement is CodedIndex testCodeIndex)
                        {
                            if (!testCodeIndex.Tag.Equals(codeIndex.Tag)) {
                                return false;
                            }
                        } else
                        {
                            return false;
                        }
                        break;
                    default:
                        var testType = testElement.GetType();
                        var curType = curElement.GetType();
                        if (!testType.Equals(curType))
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }
    }
}
