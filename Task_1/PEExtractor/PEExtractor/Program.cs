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
                Console.WriteLine("Where PEPath : path of PE file\n[args] is additional args.\"v\" for virtual loading");
                Console.ReadKey();
                return;
            }
            bool isVirtual = args.Length > 1 && args[1].Contains('v');
            try
            {
               
                using (var extractor = isVirtual ? (AbstractPECommonExtractor) new PEVirtualCommonExtractor(args[0]) : new PECommonExtractor(args[0]))
                {
                    extractor.Extract();
                    extractor.Report(args[0] + ".report");
            }
            } catch (Exception unhandledException)
            {
                Console.WriteLine("An error occured");
                Console.WriteLine(unhandledException.Message);
                Console.ReadKey();
            }
        }

    }
}
