using System;
using System.IO;

namespace DelphiToCSharpConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: DelphiToCSharpConverter <input directory> <output directory>");
                return;
            }

            string inputDir = args[0];
            string outputDir = args[1];

            var converter = new DelphiToCSharpConverter();
            converter.ConvertDirectory(inputDir, outputDir);
        }
    }
}
