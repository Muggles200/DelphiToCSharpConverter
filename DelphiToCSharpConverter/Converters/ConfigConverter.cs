using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DelphiToCSharpConverter
{
    public class ConfigConverter
    {
        public void ConvertDirectory(string inputDir, string outputDir)
        {
            foreach (string file in Directory.GetFiles(inputDir, "*.ini", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(inputDir, file);
                string outputFile = Path.Combine(outputDir, relativePath.Replace(".ini", ".json"));
                string outputFileDir = Path.GetDirectoryName(outputFile);
                if (!Directory.Exists(outputFileDir))
                {
                    Directory.CreateDirectory(outputFileDir);
                }
                ConvertFile(file, outputFile);
            }
        }

        public void ConvertFile(string inputFile, string outputFile)
        {
            string iniContent = File.ReadAllText(inputFile);
            string jsonContent = ConvertIniToJson(iniContent);
            File.WriteAllText(outputFile, jsonContent);
        }

        public string ConvertIniToJson(string iniContent)
        {
            var config = new Dictionary<string, Dictionary<string, string>>();
            var currentSection = "";

            foreach (var line in iniContent.Split('\n'))
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";"))
                {
                    continue;
                }

                var sectionMatch = Regex.Match(trimmedLine, @"^\[(.+)\]$");
                if (sectionMatch.Success)
                {
                    currentSection = sectionMatch.Groups[1].Value;
                    config[currentSection] = new Dictionary<string, string>();
                }
                else
                {
                    var keyValueMatch = Regex.Match(trimmedLine, @"^(.+?)=(.*)$");
                    if (keyValueMatch.Success)
                    {
                        var key = keyValueMatch.Groups[1].Value.Trim();
                        var value = keyValueMatch.Groups[2].Value.Trim();
                        if (!string.IsNullOrEmpty(currentSection))
                        {
                            config[currentSection][key] = value;
                        }
                    }
                }
            }

            return JsonConvert.SerializeObject(config, Formatting.Indented);
        }
    }
}
