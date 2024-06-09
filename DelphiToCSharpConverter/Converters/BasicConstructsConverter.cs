using System.Text.RegularExpressions;

namespace DelphiToCSharpConverter
{
    public class BasicConstructsConverter
    {
        public string Convert(string line)
        {
            line = Regex.Replace(line, @"\bbegin\b", "{");
            line = Regex.Replace(line, @"\bend\b", "}");
            line = Regex.Replace(line, @":=", "=");
            line = Regex.Replace(line, @"\bif\b", "if");
            line = Regex.Replace(line, @"\belse\b", "else");
            line = Regex.Replace(line, @"\bfor\b", "for");
            line = Regex.Replace(line, @"\bwhile\b", "while");
            line = Regex.Replace(line, @"\bdo\b", "");
            return line;
        }
    }
}
