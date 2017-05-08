using PEExtractor.Info;
using PEExtractor.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Tables
{
    public class TablesContext
    {
        /// <summary>
        /// Maps table names to int values
        /// </summary>
        public Dictionary<string, int> NamesToRealNum = new Dictionary<string, int>();

        /// <summary>
        /// Maps real int values to rows values from root stream structures
        /// </summary>
        public Dictionary<int, int> RealToNum = new Dictionary<int, int>();

        /// <summary>
        /// Maps rows indices from root stream structures to real int values
        /// </summary>
        public Dictionary<int, int> NumToReal = new Dictionary<int, int>();

        /// <summary>
        /// Row count of each table. (Real num to row count)
        /// </summary>
        public Dictionary<int, uint> RowCount = new Dictionary<int, uint>();

        /// <summary>
        /// Size of index of heaps (#Strings, #GUID, #Blob)
        /// </summary>
        public Dictionary<HeapType, int> HeapIndexSize = new Dictionary<HeapType, int>();

        /// <summary>
        /// Count of presented tables
        /// </summary>
        public int TableCount { get; private set; }

        private PdbStreamInfo pdbStreamInfo;

        public TablesContext(UInt64 valid, PdbStreamInfo pdbStreamInfo)
        {
            this.pdbStreamInfo = pdbStreamInfo;
            InitializeTransformIndices(valid);
        }

        private void InitializeTransformIndices(UInt64 valid)
        {
            int offset = 0;
            ///Merge with pdb information
            if (pdbStreamInfo != null)
            {
                var context = pdbStreamInfo.Context;
                
                foreach (var kvp in context.RealToNum)
                {
                    RealToNum[kvp.Key] = kvp.Value;
                }

                foreach (var kvp in context.NumToReal)
                {
                    NumToReal[kvp.Key] = kvp.Value;
                }
                offset = context.TableCount;
            }
            TableCount = offset;
            
            for (int i = 0; i < 64; i++)
            {
                if ((valid | 1) == valid)
                {
                    int num = TableCount++;
                    RealToNum[i] = num;
                    NumToReal[num] = i;
                }
                valid = valid >> 1;
            }

           
        }
        /// <summary>
        /// Initalize rows for real int tables
        /// </summary>
        /// <param name="Rows"></param>
        public void InitializeRowCount(UInt32[] Rows)
        {
            for (int i = 0; i < Rows.Length; i++)
            {
                RowCount[NumToReal[i]] = Rows[i];
            }
        }

        /// <summary>
        /// Initialize heap's index size
        /// </summary>
        /// <param name="streamRoot"></param>
        public void InitializeHeapIndexSize(TildeStreamRoot streamRoot)
        {
            int heapSizes = streamRoot.HeapSizes;

            if ((heapSizes | 1) == heapSizes)
            {
                HeapIndexSize[HeapType.Strings] = 4;
            }
            else
            {
                HeapIndexSize[HeapType.Strings] = 2;
            }
            heapSizes = heapSizes >> 1;

            if ((heapSizes | 1) == heapSizes)
            {
                HeapIndexSize[HeapType.GUID] = 4;
            }
            else
            {
                HeapIndexSize[HeapType.GUID] = 2;
            }
            heapSizes = heapSizes >> 1;

            if ((heapSizes | 1) == heapSizes)
            {
                HeapIndexSize[HeapType.Blob] = 4;
            }
            else
            {
                HeapIndexSize[HeapType.Blob] = 2;
            }
            heapSizes = heapSizes >> 1;
        }
    }
}
