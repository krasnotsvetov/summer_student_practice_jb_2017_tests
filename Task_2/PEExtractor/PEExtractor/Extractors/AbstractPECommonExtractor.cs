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
    public abstract class AbstractPECommonExtractor : IDisposable
    {

        protected const int DebugDataDirectory = 6;
        protected const int CLRRuntimeHeaderDataDirectory = 14;


        protected const int ImageNumberOfDirectoryEntries = 16;

        /// <summary>
        /// Image file header
        /// </summary>
        public ImageFileHeader ImageFileHeader                                           { get; protected set; }

        /// <summary>
        /// Image optional header. Like haskell either
        /// </summary>
        public IEither<ImageOptionalHeader32, ImageOptionalHeader64> ImageOptionalHeader { get; protected set; }

        /// <summary>
        /// Data directories
        /// </summary>
        public ImageDataDirectory[] DataDirectories                                      { get; protected set; } = new ImageDataDirectory[ImageNumberOfDirectoryEntries];

        /// <summary>
        /// Section headers
        /// </summary>
        public ImageSectionHeader[] SectionHeaders                                       { get; protected set; }



        /// <summary>
        /// BSJB information
        /// </summary>
        public BSJBInfo BSJBInfo { get; protected set; }


        /// <summary>
        /// #~ Stream information with all tables.
        /// </summary>
        public TildeStreamInfo TildeStreamInfo { get; protected set; }

        /// <summary>
        /// #Pdb stream information
        /// </summary>
        public PdbStreamInfo PdbStreamInfo { get; protected set; }

        /// <summary>
        /// Path to library
        /// </summary>
        public string PEPath     { get; protected set; }



        protected AbstractPECommonExtractor()
        {

        }


        public abstract void Extract();

        /// <summary>
        /// Check MZ signature
        /// </summary>
        /// <param name="block">a block which contains signature</param>
        protected void CheckMZ(IBlock block)
        {
            var reader = block.BlockReader;
            reader.CheckSignature("MZ");
        }

        /// <summary>
        /// Check PE signature
        /// </summary>
        /// <param name="block">a block which contains signature</param>
        protected void CheckPE00Signature(IBlock block)
        {
            var reader = block.BlockReader;
            reader.CheckSignature("PE\0\0");
        }

        /// <summary>
        /// Initialize image file header
        /// </summary>
        /// <param name="block">byte block which contains image file header</param>
        protected void InitializeImageFileHeader(IBlock block)
        {
            var reader = block.BlockReader;
            ImageFileHeader = reader.ReadStruct<ImageFileHeader>();
        }


        /// <summary>
        ///  Initialize image optional header
        /// </summary>
        /// <param name="block">byte block which contains image optional header</param>
        /// <param name="magic">an number which show is 32 or 64 bit system</param>
        protected void InitializeImageOptionalHeader(IBlock block, int magic)
        {
            switch (magic)
            {
                case 0x10B:
                    ImageOptionalHeader = Either.Left<ImageOptionalHeader32, ImageOptionalHeader64>(block.BlockReader.ReadStruct<ImageOptionalHeader32>());
                    break;
                case 0x20B:
                    ImageOptionalHeader = Either.Right<ImageOptionalHeader32, ImageOptionalHeader64>(block.BlockReader.ReadStruct<ImageOptionalHeader64>());
                    break;
                default:
                    throw new SignatureException($"Unexcepted magic in optional header. The value is : {magic}");
            }
        }

        /// <summary>
        /// Initialize data directories
        /// </summary>
        /// <param name="block">byte block which contains data directories</param>
        protected void InitializeDataDirectories(IBlock block)
        {
            var reader = block.BlockReader;
            for (int i = 0; i < ImageNumberOfDirectoryEntries; i++)
            {
                DataDirectories[i] = reader.ReadStruct<ImageDataDirectory>();
            }
        }

        /// <summary>
        /// Initialize section headers
        /// </summary>
        /// <param name="block">byte block which contains section headers</param>
        protected void InitializeSectionHeaders(IBlock block)
        {
            var reader = block.BlockReader;
            SectionHeaders = new ImageSectionHeader[ImageFileHeader.NumberOfSections];
            for (int i = 0; i < SectionHeaders.Length; i++)
            {
                SectionHeaders[i] = reader.ReadStruct<ImageSectionHeader>();
            }
        }


        ///BSJB


        /// <summary>
        /// Extract BSJB information
        /// </summary>
        protected abstract void ExtractBSJB();

        /// <summary>
        /// Extract only BSJB 
        /// </summary>
        /// <param name="BSJBRowPointer">Pointer to BSJB signature</param>
        public abstract void ExtractOnlyBSJB(uint BSJBRowPointer);


        /// <summary>
        /// Extreact #pdb stream & #~ stream
        /// </summary>
        protected void ExtractStreams()
        {
            ExtractPDBStream();
            ExtractTildeStream();
        }

        /// <summary>
        /// Extract #Pdb stream
        /// </summary>
        protected abstract void ExtractPDBStream();

        /// <summary>
        /// Extract #~ stream with tables
        /// </summary>
        protected abstract void ExtractTildeStream();


        /// <summary>
        /// Extract CLR header
        /// </summary>
        /// <param name="block">block which contains CLR header</param>
        /// <returns></returns>
        protected ImageCor20Header ExtractImageCor20Header(IBlock block)
        {
            return block.BlockReader.ReadStruct<ImageCor20Header>();
        }


        /// <summary>
        /// Return a index of stream headers which has same name
        /// </summary>
        /// <param name="name">stream name</param>
        /// <returns></returns>
        protected int GetStreamIndex(string name)
        {
            for (int i = 0; i < BSJBInfo.Metadata.StreamHeaders.Length; i++)
            {
                var sh = BSJBInfo.Metadata.StreamHeaders[i];
                if (sh.Name.Equals(name))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Report information to log file
        /// </summary>
        public virtual void Report()
        {
            if (PdbStreamInfo != null)
            {
                using (var sw = new StreamWriter(new FileStream($"{PEPath}.PDBStream.Report", FileMode.Create)))
                {
                    PdbStreamInfo.Report(sw);
                }
            }

            if (TildeStreamInfo != null)
            {
                using (var sw = new StreamWriter(new FileStream($"{PEPath}.~Stream.Report", FileMode.Create)))
                {
                    TildeStreamInfo.Report(sw);

                    sw.WriteLine($"Tables at { PEPath}.TablesReport");
                }

                using (var sw = new StreamWriter(new FileStream($"{PEPath}.Tables.Report", FileMode.Create)))
                {
                    foreach (var t in TildeStreamInfo.Tables.Values)
                    {
                        t.Report(sw);
                        sw.WriteLine();
                        sw.WriteLine();
                        sw.WriteLine();
                    }
                }
            }

            if (BSJBInfo != null)
            {
                using (var sw = new StreamWriter(new FileStream($"{PEPath}.BSJBMetadata.Report", FileMode.Create)))
                {
                    BSJBInfo.Report(sw);
                }
            }
        }

        public abstract void Dispose();
    }
}
