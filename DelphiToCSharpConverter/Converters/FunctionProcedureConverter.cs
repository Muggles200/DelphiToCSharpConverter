using System.Text.RegularExpressions;

namespace DelphiToCSharpConverter
{
    public class FunctionProcedureConverter
    {
        public string Convert(string line)
        {
            line = Regex.Replace(line, @"procedure\s+(\w+)\s*\((.*?)\);", "void $1($2);");
            line = Regex.Replace(line, @"procedure\s+(\w+);", "void $1();");
            line = Regex.Replace(line, @"function\s+(\w+)\s*\((.*?)\)\s*:\s*(\w+);", "$3 $1($2);");
            line = Regex.Replace(line, @"function\s+(\w+)\s*:\s*(\w+);", "$2 $1();");
            line = Regex.Replace(line, @"function\s+(\w+)\s*\((.*?)\)\s*:\s*(\w+)\s*override;", "override $3 $1($2);");
            line = Regex.Replace(line, @"function\s+(\w+)\s*:\s*(\w+)\s*override;", "override $2 $1();");
            return line;
        }
    }
}
