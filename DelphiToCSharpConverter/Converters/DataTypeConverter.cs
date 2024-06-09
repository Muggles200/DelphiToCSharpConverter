using System.Text.RegularExpressions;

namespace DelphiToCSharpConverter
{
    public class DataTypeConverter
    {
        public string Convert(string line)
        {
            line = Regex.Replace(line, @"\binteger\b", "int");
            line = Regex.Replace(line, @"\breal\b", "float");
            line = Regex.Replace(line, @"\bstring\b", "string");
            line = Regex.Replace(line, @"\bboolean\b", "bool");
            line = Regex.Replace(line, @"\bchar\b", "char");
            line = Regex.Replace(line, @"\bvariant\b", "object");
            return line;
        }
    }
}
