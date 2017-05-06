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

        private LibraryHandle libHandle;

        /// <summary>
        /// Image base of pe file when it loaded
        /// </summary>
        public IntPtr ImageBase { get; private set; }


        /// <summary>
        /// Execute library and extract information
        /// </summary>
        /// <param name="PEPath">Path to library</param>
        public PEVirtualCommonExtractor(string PEPath)
        {
            this.PEPath = PEPath;
            libHandle = new LibraryHandle(PEPath);
            ImageBase = libHandle.Handle;
        }

        /// <summary>
        /// Extract headers, BSJB and debug information from PE file
        /// </summary>
        public override void Extract()
        {
            uint curOffset = 0;
            using (var block = new MemoryBlock(ImageBase, 2))
            {
                CheckMZ(block);
            }

            uint e_lfnew = 0;
            using (var block = new MemoryBlock(ImageBase + 0x3c, 4))
            {
                e_lfnew = block.BlockReader.ReadUInt32();
            }

            using (var block = new MemoryBlock(VAAddress(e_lfnew), 4))
            {
                CheckPE00Signature(block);
            }

            curOffset = e_lfnew + 4;

            using (var block = new MemoryBlock(VAAddress(e_lfnew + 4), 20))
            {
                InitializeImageFileHeader(block);
            }

            curOffset += (uint)Marshal.SizeOf(typeof(ImageFileHeader));

            Int16 magic;

            using (var block = new MemoryBlock(VAAddress(curOffset), 2))
            {
                magic = block.BlockReader.ReadInt16();
            }

            uint size = magic == 0x10B ? (uint)Marshal.SizeOf(typeof(ImageOptionalHeader32)) : (uint)Marshal.SizeOf(typeof(ImageOptionalHeader64));
            using (var block = new MemoryBlock(VAAddress(curOffset), size))
            {
                InitializeImageOptionalHeader(block, magic);
            }
            curOffset += size;

            size = (uint)(DataDirectories.Length * Marshal.SizeOf(typeof(ImageDataDirectory)));
            using (var block = new MemoryBlock(VAAddress(curOffset), size))
            {
                InitializeDataDirectories(block);
            }

            curOffset += size;

            size = (uint)(ImageFileHeader.NumberOfSections * Marshal.SizeOf(typeof(ImageSectionHeader)));
            using (var block = new MemoryBlock(VAAddress(curOffset), size))
            {
                InitializeSectionHeaders(block);
            }
            ExtractBSJB();
            ExtractDebug();
        } 


        /// <summary>
        /// Extract BSJB section from PE file
        /// </summary>
        protected override void ExtractBSJB()
        {
            if (DataDirectories[CLRRuntimeHeaderDataDirectory].VirtualAddress == 0)
            {
                //Notify, that library doesn't contain CLR 
            }
            else
            {

                using (var block = new MemoryBlock(VAAddress(DataDirectories[CLRRuntimeHeaderDataDirectory].VirtualAddress), DataDirectories[CLRRuntimeHeaderDataDirectory].Size))
                {
                    BSJBInfo = new BSJBInfo(ExtractImageCor20Header(block));
                }

                using (var block = new MemoryBlock(VAAddress(BSJBInfo.ImageCor20Header.MetaData.VirtualAddress), BSJBInfo.ImageCor20Header.MetaData.Size))
                {
                    BSJBInfo.Extract(block);
                }

            }
        }


        /// <summary>
        /// Convert rva to va address
        /// </summary>
        /// <param name="rva">rva address</param>
        /// <returns></returns>
        private IntPtr VAAddress(uint rva)
        {
            if (IntPtr.Size == 4)
            {
                return new IntPtr(ImageBase.ToInt32() + rva);
            } else
            {
                return new IntPtr(ImageBase.ToInt64() + rva);
            }
        }
        
        public override void Dispose()
        {
            libHandle.Dispose();
        }
    }
}
