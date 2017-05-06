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
    public sealed partial class PEVirtualCommonExtractor : AbstractPECommonExtractor
    {

        /// <summary>
        /// Extract debug section from PE file
        /// </summary>
        protected override void ExtractDebug()
        {
            if (DataDirectories[DebugDataDirectory].VirtualAddress == 0)
            {
                //Notify, that library doesn't contain debug 
            }
            else
            {

                for (int i = 0; i < DataDirectories[DebugDataDirectory].Size / Marshal.SizeOf(typeof(ImageDebugDirectory)); i++)
                {
                    ImageDebugDirectory debugEntry;
                    uint rva = DataDirectories[DebugDataDirectory].VirtualAddress + (uint)(i * Marshal.SizeOf(typeof(ImageDebugDirectory)));
                    using (var block = new MemoryBlock(VAAddress(rva), (uint)Marshal.SizeOf(typeof(ImageDebugDirectory))))
                    {
                        debugEntry = block.BlockReader.ReadStruct<ImageDebugDirectory>();
                    }
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
        protected override void ExtractMPDB(ImageDebugDirectory debugEntry)
        {
            MPDBInfo = new MPDBInfo(debugEntry);
            if (debugEntry.AddressOfRawData == 0)
            {
                MPDBInfo.ReportInformation = "The debug information is not covered by a section header";
                return;
            }
            using (var block = new MemoryBlock(VAAddress(debugEntry.AddressOfRawData), debugEntry.SizeOfData))
            {
                MPDBInfo.Extract(block);
            }
        }


        /// <summary>
        /// Extract RSDS section from PE file
        /// </summary>
        protected override void ExtractRSDS(ImageDebugDirectory debugEntry)
        {
            RSDSInfo = new RSDSInfo(debugEntry);
            if (debugEntry.AddressOfRawData == 0)
            {
                RSDSInfo.ReportInformation = "The debug information is not covered by a section header";
                return;
            }
            using (var block = new MemoryBlock(VAAddress(debugEntry.AddressOfRawData), debugEntry.SizeOfData))
            {
                RSDSInfo.Extract(block);
            }
        }
    }
}
