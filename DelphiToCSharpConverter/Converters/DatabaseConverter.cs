using System.Text.RegularExpressions;

namespace DelphiToCSharpConverter
{
    public class DatabaseConverter
    {
        public string Convert(string line)
        {
            line = Regex.Replace(line, @"TFDConnection", "SqlConnection");
            line = Regex.Replace(line, @"TFDQuery", "SqlCommand");
            return line;
        }
    }
}
