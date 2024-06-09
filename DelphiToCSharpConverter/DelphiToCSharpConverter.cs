using System;
using System.IO;

namespace DelphiToCSharpConverter
{
    public class DelphiToCSharpConverter
    {
        private readonly BasicConstructsConverter basicConverter = new BasicConstructsConverter();
        private readonly FunctionProcedureConverter functionConverter = new FunctionProcedureConverter();
        private readonly DataTypeConverter dataTypeConverter = new DataTypeConverter();
        private readonly ControlStructureConverter controlStructureConverter = new ControlStructureConverter();
        private readonly ClassConverter classConverter = new ClassConverter();
        private readonly ExceptionHandlingConverter exceptionConverter = new ExceptionHandlingConverter();
        private readonly LibraryConverter libraryConverter = new LibraryConverter();
        private readonly DatabaseConverter databaseConverter = new DatabaseConverter();
        private readonly ConfigConverter configConverter = new ConfigConverter();

        public void ConvertFile(string inputFile, string outputFile)
        {
            string delphiCode = File.ReadAllText(inputFile);
            string csharpCode = ConvertCode(delphiCode);
            File.WriteAllText(outputFile, csharpCode);
        }

        public void ConvertDirectory(string inputDir, string outputDir)
        {
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            foreach (string file in Directory.GetFiles(inputDir, "*.pas", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(inputDir, file);
                string outputFile = Path.Combine(outputDir, relativePath.Replace(".pas", ".cs"));
                string outputFileDir = Path.GetDirectoryName(outputFile);
                if (!Directory.Exists(outputFileDir))
                {
                    Directory.CreateDirectory(outputFileDir);
                }
                ConvertFile(file, outputFile);
            }

            // Convert configuration files
            configConverter.ConvertDirectory(inputDir, outputDir);
        }

        public string ConvertCode(string delphiCode)
        {
            var lines = delphiCode.Split('\n');
            var csharpCode = string.Empty;

            foreach (var line in lines)
            {
                var convertedLine = line.Trim();
                convertedLine = libraryConverter.Convert(convertedLine);
                convertedLine = basicConverter.Convert(convertedLine);
                convertedLine = functionConverter.Convert(convertedLine);
                convertedLine = dataTypeConverter.Convert(convertedLine);
                convertedLine = controlStructureConverter.Convert(convertedLine);
                convertedLine = classConverter.Convert(convertedLine);
                convertedLine = exceptionConverter.Convert(convertedLine);
                convertedLine = databaseConverter.Convert(convertedLine);
                csharpCode += convertedLine + Environment.NewLine;
            }

            return csharpCode;
        }
    }
}
