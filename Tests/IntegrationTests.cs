using System;
using System.IO;
using NUnit.Framework;
using DelphiToCSharpConverter;

namespace Tests
{
    public class IntegrationTests
    {
        private Converter converter;
        private string inputDir;
        private string expectedOutputDir;

        [SetUp]
        public void Setup()
        {
            converter = new Converter();
            inputDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "delphi_samples");
            expectedOutputDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "expected_csharp_output");
        }

        private bool CompareFiles(string file1, string file2)
        {
            return File.ReadAllText(file1).Trim() == File.ReadAllText(file2).Trim();
        }

        [Test]
        public void TestSample2()
        {
            string inputFile = Path.Combine(inputDir, "sample2.pas");
            string expectedOutputFile = Path.Combine(expectedOutputDir, "sample2.cs");
            string outputFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "output_sample2.cs");

            converter.ConvertFile(inputFile, outputFile);
            Assert.IsTrue(CompareFiles(outputFile, expectedOutputFile));
            File.Delete(outputFile);
        }
    }
}
