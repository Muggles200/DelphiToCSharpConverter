using System.Text.RegularExpressions;

namespace DelphiToCSharpConverter
{
    public class ClassConverter
    {
        public string Convert(string line)
        {
            line = Regex.Replace(line, @"class\s+(\w+)\s*=\s*class\s*\((\w+)\)", "class $1 : $2");
            line = Regex.Replace(line, @"constructor\s+(\w+);", "public $1()");
            line = Regex.Replace(line, @"destructor\s+(\w+);", "protected $1()");
            line = Regex.Replace(line, @"procedure\s+(\w+);", "void $1()");
            line = Regex.Replace(line, @"function\s+(\w+);\s*(\w+);", "$2 $1()");
            return line;
        }
    }
}
