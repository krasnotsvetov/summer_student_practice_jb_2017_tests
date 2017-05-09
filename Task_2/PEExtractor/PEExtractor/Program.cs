using PEExtractor.Extractors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"Usage : <PEPath> [args]");
                Console.WriteLine("Where PEPath : path of PE file\n[args] is additional args.\n For example use \"-f\" from full PE file or " +
                                  "skip additional arguments if the file contains only BSJB");
                Console.ReadKey();
                return;
            }
            bool isFileFull = args.Length > 1 && args[1].Contains('f');
            try
            {
               
                using (var extractor = new PECommonExtractor(args[0]))
                {
                    if (isFileFull)
                    {
                        extractor.Extract();
                    }
                    else
                    {
                        extractor.ExtractOnlyBSJB(0);
                    }
                    extractor.Report();
                }
            } 
            catch (Exception unhandledException)
            {
                  Console.WriteLine("An error occured");
                  Console.WriteLine(unhandledException.Message);
                  Console.ReadKey();
            }
        }

    }
}
