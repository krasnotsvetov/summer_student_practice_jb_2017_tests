using PEExtractor.Common;
using PEExtractor.Common.Extension;
using PEExtractor.Exceptions;
using PEExtractor.Extractors.DataBlock;
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

        private BinaryReader reader;

        /// <summary>
        /// Open a library file to read(Not load and execute in memory)
        /// </summary>
        /// <param name="pePath">path to library file</param>
        public PECommonExtractor(string pePath)
        {
            PEPath = pePath;
            reader = new BinaryReader(new FileStream(pePath, FileMode.Open));
        }

        /// <summary>
        /// Extract headers, BSJB and debug information
        /// </summary>
        public override void Extract()
        {
            using (var block = new FileBlock(reader, 2, 0, true))
            {
                CheckMZ(block);
            }
            //Move to e_lfnew
            reader.SetStreamPosition(0x3c);
            uint e_lfnew = reader.ReadUInt32();
            using (var block = new FileBlock(reader, 4, e_lfnew, true))
            {
                CheckPE00Signature(block);
            }

            using (var block = new FileBlock(reader, 20, (uint)reader.BaseStream.Position, true))
            {
                InitializeImageFileHeader(block);
            }

            Int16 magic = reader.ReadInt16();
            //need to peek int16
            reader.BaseStream.Position -= 2;

            uint size = magic == 0x10B ? (uint)Marshal.SizeOf(typeof(ImageOptionalHeader32)) : (uint)Marshal.SizeOf(typeof(ImageOptionalHeader64));
            using (var block = new FileBlock(reader, size, (uint)reader.BaseStream.Position, true))
            {
                InitializeImageOptionalHeader(block, magic);
            }

            using (var block = new FileBlock(reader, (uint)(DataDirectories.Length * Marshal.SizeOf(typeof(ImageDataDirectory))), (uint)reader.BaseStream.Position, true))
            {
                InitializeDataDirectories(block);
            }

            using (var block = new FileBlock(reader, (uint)(ImageFileHeader.NumberOfSections * Marshal.SizeOf(typeof(ImageSectionHeader))), (uint)reader.BaseStream.Position, true))
            {
                InitializeSectionHeaders(block);
            }


            ExtractBSJB();
            ExtractDebug();
        }



        /// <summary>
        /// Convert RVA to RAW pointer
        /// </summary>
        /// <param name="RVAPointer"></param>
        /// <param name="sectionIndex"></param>
        /// <returns></returns>
        private uint ConvertToRawPointer(uint RVAPointer, out int sectionIndex)
        {
            uint sectionAlligment = ImageOptionalHeader.Case(t => t.SectionAlignment, t => t.SectionAlignment);

            sectionIndex = -1;
            for (int i = 0; i < SectionHeaders.Length; i++)
            {
                uint sectionSize = SectionHeaders[i].VirtualSize;
                uint count = (uint)(sectionSize / sectionAlligment);
                uint size = count * sectionAlligment < sectionSize ? (count + 1) * sectionAlligment : count * sectionAlligment;

                if (RVAPointer >= SectionHeaders[i].VirtualAddress && RVAPointer < SectionHeaders[i].VirtualAddress + size)
                {
                    sectionIndex = i;
                    break;
                }
            }
            if (sectionIndex == -1) return 0;

            return RVAPointer - SectionHeaders[sectionIndex].VirtualAddress + SectionHeaders[sectionIndex].PointerToRawData;
        }

        public override void Dispose()
        {
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
        }
    }
}
