using PEExtractor.Common;
using PEExtractor.Common.Extension;
using PEExtractor.Exceptions;
using PEExtractor.Extractors.DataBlock;
using PEExtractor.Info;
using PEExtractor.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Extractors
{
    public sealed partial class PECommonExtractor : AbstractPECommonExtractor
    {
        /// <summary>
        /// Extract debug information from PE file
        /// </summary>
        protected override void ExtractDebug()
        {
            if (DataDirectories[DebugDataDirectory].VirtualAddress == 0)
            {
                //Notify, that library doesn't contain debug 
            }
            else
            {

                uint RawDebugDataPointer = ConvertToRawPointer(DataDirectories[DebugDataDirectory].VirtualAddress, out int sectionIndex);
                for (int i = 0; i < DataDirectories[DebugDataDirectory].Size / Marshal.SizeOf(typeof(ImageDebugDirectory)); i++)
                {
                    reader.SetStreamPosition(RawDebugDataPointer + i * Marshal.SizeOf(typeof(ImageDebugDirectory)));
                    var debugEntry = reader.ReadStruct<ImageDebugDirectory>();
                    switch (debugEntry.Type)
                    {
                        case 2:
                            ExtractRSDS(debugEntry);
                            break;
                        case 16:
                            //not required in task. More than, need pdb file to reproduce
                            break;
                        case 17:
                            ExtractMPDB(debugEntry);
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Extract MPDB section from PE file
        /// </summary>
        /// <param name="debugEntry">debug entry whitch contains pointer to MPDB</param>
        protected override void ExtractMPDB(ImageDebugDirectory debugEntry)
        {
            MPDBInfo = new MPDBInfo(debugEntry);
            using (var block = new FileBlock(reader, debugEntry.SizeOfData, debugEntry.PointerToRawData, true))
            {
                MPDBInfo.Extract(block);
            }
        }

        /// <summary>
        /// Extract MPDB section from PE file
        /// </summary>
        /// <param name="debugEntry">debug entry whitch contains pointer to RSDS</param>
        protected override void ExtractRSDS(ImageDebugDirectory debugEntry)
        {
            RSDSInfo = new RSDSInfo(debugEntry);
            using (var block = new FileBlock(reader, debugEntry.SizeOfData, debugEntry.PointerToRawData, true))
            {
                RSDSInfo.Extract(block);
            }
        }

    }
}
