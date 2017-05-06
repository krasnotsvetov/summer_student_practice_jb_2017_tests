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
        /// MPDB information
        /// </summary>
        public MPDBInfo MPDBInfo { get; protected set; }

        /// <summary>
        /// RSDS information
        /// </summary>
        public RSDSInfo RSDSInfo { get; protected set; }


        /// <summary>
        /// BSJB information
        /// </summary>
        public BSJBInfo BSJBInfo { get; protected set; }

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

        protected abstract void ExtractBSJB();

        ///Debug

        protected ImageCor20Header ExtractImageCor20Header(IBlock block)
        {
            return block.BlockReader.ReadStruct<ImageCor20Header>();
        }

        protected abstract void ExtractDebug();
        protected abstract void ExtractMPDB(ImageDebugDirectory debugEntry);
        protected abstract void ExtractRSDS(ImageDebugDirectory debugEntry);


        /// <summary>
        /// Report information to log file
        /// </summary>
        /// <param name="reportPath">path of log file</param>
        public virtual void Report(string reportPath)
        {
            using (var sw = new StreamWriter(new FileStream(reportPath, FileMode.Create)))
            {
                sw.WriteLine($"PE name : {PEPath}");

                sw.WriteLine($"Image File Header:");
                StructReporter.Report(ImageFileHeader, sw, 1);

                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine($"Optional File Header:");
                ImageOptionalHeader.Case(t => StructReporter.Report(t, sw, 1), t => StructReporter.Report(t, sw, 1));

                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine($"Data directories:");

                for (int i = 0; i < DataDirectories.Length; i++)
                {
                    StructReporter.Report(DataDirectories[i], sw, 1);
                    sw.WriteLine();
                }

                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine($"Section headers:");

                for (int i = 0; i < SectionHeaders.Length; i++)
                {

                    StringBuilder name = new StringBuilder();
                    unsafe
                    {
                        fixed (byte* t = SectionHeaders[i].Name)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                name.Append((char)t[j]);
                            }
                        }
                    }
                    sw.WriteLine($"\tName : {name.ToString()}");
                    StructReporter.Report(SectionHeaders[i], sw, 1, new SortedSet<string>() { "Name" });
                    sw.WriteLine();
                }

                sw.WriteLine();
                sw.WriteLine();

                if (BSJBInfo != null)
                {
                    BSJBInfo.Report(sw);
                }

                if (RSDSInfo != null)
                {
                    RSDSInfo.Report(sw);
                }

                if (MPDBInfo != null)
                {
                    MPDBInfo.Report(sw);
                }
            }
        }

        public abstract void Dispose();
    }
}
