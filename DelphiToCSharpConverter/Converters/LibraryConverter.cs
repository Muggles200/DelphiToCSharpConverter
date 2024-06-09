namespace DelphiToCSharpConverter
{
    public class LibraryConverter
    {
        public string Convert(string line)
        {
            line = line.Replace("uses Windows", "using System;");
            line = line.Replace("uses Messages", "using System;");
            line = line.Replace("uses SysUtils", "using System;");
            line = line.Replace("uses Variants", "using System;");
            line = line.Replace("uses Classes", "using System.Collections;");
            line = line.Replace("uses Graphics", "using System.Drawing;");
            line = line.Replace("uses Controls", "using System.Windows.Forms;");
            line = line.Replace("uses Forms", "using System.Windows.Forms;");
            line = line.Replace("uses Dialogs", "using System.Windows.Forms;");
            line = line.Replace("uses DB", "using System.Data;");
            line = line.Replace("uses ADODB", "using System.Data.SqlClient;");
            line = line.Replace("uses ExtCtrls", "using System.Windows.Forms;");
            line = line.Replace("uses StdCtrls", "using System.Windows.Forms;");
            line = line.Replace("uses ComCtrls", "using System.Windows.Forms;");
            line = line.Replace("uses u_mlc_Message_Log_Control", "using MessageLogControl;");
            line = line.Replace("uses Buttons", "using System.Windows.Forms;");
            return line;
        }
    }
}
