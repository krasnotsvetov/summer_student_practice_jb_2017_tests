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

        /// <summary>
        /// Extract only BSJB. 
        /// </summary>
        /// <param name="BSJBRowPointer">Pointer to BSJB signature</param>
        public override void ExtractOnlyBSJB(uint BSJBRowPointer)
        {
            BSJBInfo = new BSJBInfo(new ImageCor20Header());
            using (var block = new FileBlock(reader, 0, BSJBRowPointer, true))
            {
                BSJBInfo.Extract(block);
            }
            ExtractStreams();
        }


        /// <summary>
        /// Extract #~ stream information
        /// </summary>
        protected override void ExtractPDBStream()
        {
            int index = GetStreamIndex("#Pdb");
            if (index == -1) return;

            var pdbStreamHeader = BSJBInfo.Metadata.StreamHeaders[index];
            using (var block = new FileBlock(reader, pdbStreamHeader.Size, pdbStreamHeader.Offset + BSJBInfo.MetadataRAWPointer, true))
            {
                PdbStreamInfo = new PdbStreamInfo();
                PdbStreamInfo.Extract(block);
            }
        }


        /// <summary>
        /// Extract #~ stream information
        /// </summary>
        protected override void ExtractTildeStream()
        {
            int index = GetStreamIndex("#~");
            if (index == -1)
            {
                return;
            }
            var tildeStreamHeader = BSJBInfo.Metadata.StreamHeaders[index];
            using (var block = new FileBlock(reader, tildeStreamHeader.Size, tildeStreamHeader.Offset + BSJBInfo.MetadataRAWPointer, true))
            {
                TildeStreamInfo = new TildeStreamInfo(PdbStreamInfo);
                TildeStreamInfo.Extract(block);
            }
        }

       
    }
}
