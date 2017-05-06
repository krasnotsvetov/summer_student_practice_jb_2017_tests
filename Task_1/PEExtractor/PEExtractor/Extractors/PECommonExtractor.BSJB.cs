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
    public partial class PECommonExtractor : AbstractPECommonExtractor
    {

        /// <summary>
        /// Extract BSJB information with metadata
        /// </summary>
        protected override void ExtractBSJB()
        {
            if (DataDirectories[CLRRuntimeHeaderDataDirectory].VirtualAddress == 0)
            {
                //Notify, that library doesn't contain CLR 
            }
            else
            {
                uint BSJBRawPointer = ConvertToRawPointer(DataDirectories[CLRRuntimeHeaderDataDirectory].VirtualAddress, out int sectionIndex);
                
                using(var block = new FileBlock(reader, (uint)Marshal.SizeOf(typeof(ImageCor20Header)), BSJBRawPointer, true))
                {
                    BSJBInfo = new BSJBInfo(ExtractImageCor20Header(block));
                }



                uint metadataRootPointer = BSJBInfo.ImageCor20Header.MetaData.VirtualAddress - SectionHeaders[sectionIndex].VirtualAddress
                                                                                             + SectionHeaders[sectionIndex].PointerToRawData;
                using (var block = new FileBlock(reader, BSJBInfo.ImageCor20Header.MetaData.Size, metadataRootPointer, true))
                {
                    BSJBInfo.Extract(block);
                }
            }
        }


    

    }
}
