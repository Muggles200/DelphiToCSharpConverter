using System.Text.RegularExpressions;

namespace DelphiToCSharpConverter
{
    public class ExceptionHandlingConverter
    {
        public string Convert(string line)
        {
            line = Regex.Replace(line, @"raise\s+(\w+);", "throw $1;");
            line = Regex.Replace(line, @"try\s*begin", "try");
            line = Regex.Replace(line, @"except\s*begin", "catch");
            line = Regex.Replace(line, @"finally\s*begin", "finally");
            return line;
        }
    }
}
