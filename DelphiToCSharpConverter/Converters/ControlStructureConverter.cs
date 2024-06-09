using System.Text.RegularExpressions;

namespace DelphiToCSharpConverter
{
    public class ControlStructureConverter
    {
        public string Convert(string line)
        {
            line = Regex.Replace(line, @"case\s+(\w+)\s+of", "switch ($1)");
            line = Regex.Replace(line, @"with\s+(\w+)\s+do", "using ($1)");
            return line;
        }
    }
}
