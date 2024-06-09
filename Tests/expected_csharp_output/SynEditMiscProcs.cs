using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace SynEditMiscProcs
{
    public static class SynEditMiscProcs
    {
        public const int MaxIntArraySize = int.MaxValue / 16;

        public static int[] GetIntArray(uint count, int initialValue)
        {
            var result = new int[count];
            if (initialValue != 0)
            {
                for (var i = 0; i < count; i++)
                {
                    result[i] = initialValue;
                }
            }
            return result;
        }

        public static int MinMax(int x, int mi, int ma)
        {
            x = Math.Min(x, ma);
            return Math.Max(x, mi);
        }

        public static void SwapInt(ref int l, ref int r)
        {
            var tmp = r;
            r = l;
            l = tmp;
        }

        public static Point MaxPoint(Point p1, Point p2)
        {
            return (p2.Y > p1.Y) || ((p2.Y == p1.Y) && (p2.X > p1.X)) ? p2 : p1;
        }

        public static Point MinPoint(Point p1, Point p2)
        {
            return (p2.Y < p1.Y) || ((p2.Y == p1.Y) && (p2.X < p1.X)) ? p2 : p1;
        }

        public static void InternalFillRect(IntPtr hdc, Rectangle rcPaint)
        {
            using (var graphics = Graphics.FromHdc(hdc))
            {
                graphics.FillRectangle(Brushes.White, rcPaint);
            }
        }

        private static bool GetHasTabs(char[] line, out int charsBefore)
        {
            charsBefore = 0;
            if (line != null)
            {
                foreach (var ch in line)
                {
                    if (ch == '\t')
                        break;
                    charsBefore++;
                }
                return Array.IndexOf(line, '\t') >= 0;
            }
            return false;
        }

        public static string ConvertTabs1Ex(string line, int tabWidth, out bool hasTabs)
        {
            if (GetHasTabs(line.ToCharArray(), out int nBeforeTab))
            {
                hasTabs = true;
                var result = new char[line.Length];
                Array.Copy(line.ToCharArray(), result, line.Length);
                for (var i = nBeforeTab; i < result.Length; i++)
                {
                    if (result[i] == '\t')
                        result[i] = ' ';
                }
                return new string(result);
            }
            hasTabs = false;
            return line;
        }

        public static string ConvertTabs1(string line, int tabWidth)
        {
            return ConvertTabs1Ex(line, tabWidth, out _);
        }

        public static string ConvertTabs2nEx(string line, int tabWidth, out bool hasTabs)
        {
            if (GetHasTabs(line.ToCharArray(), out int destLen))
            {
                hasTabs = true;
                var tabMask = (tabWidth - 1) ^ 0x7FFFFFFF;
                var tabCount = 0;

                foreach (var ch in line)
                {
                    if (ch == '\t')
                    {
                        destLen = (destLen + tabWidth) & tabMask;
                        tabCount++;
                    }
                    else
                        destLen++;
                }

                var result = new char[destLen];
                var pSrc = line.ToCharArray();
                var pDest = 0;

                foreach (var ch in pSrc)
                {
                    if (ch == '\t')
                    {
                        var i = tabWidth - (pDest % tabWidth);
                        for (var j = 0; j < i; j++)
                        {
                            result[pDest++] = '\t';
                        }
                    }
                    else
                    {
                        result[pDest++] = ch;
                    }
                }
                return new string(result);
            }
            hasTabs = false;
            return line;
        }

        public static string ConvertTabs2n(string line, int tabWidth)
        {
            return ConvertTabs2nEx(line, tabWidth, out _);
        }

        public static string ConvertTabsEx(string line, int tabWidth, out bool hasTabs)
        {
            if (GetHasTabs(line.ToCharArray(), out int destLen))
            {
                hasTabs = true;
                var tabCount = 0;

                foreach (var ch in line)
                {
                    if (ch == '\t')
                    {
                        destLen += tabWidth - (destLen % tabWidth);
                        tabCount++;
                    }
                    else
                        destLen++;
                }

                var result = new char[destLen];
                var pSrc = line.ToCharArray();
                var pDest = 0;

                foreach (var ch in pSrc)
                {
                    if (ch == '\t')
                    {
                        var i = tabWidth - (pDest % tabWidth);
                        for (var j = 0; j < i; j++)
                        {
                            result[pDest++] = '\t';
                        }
                    }
                    else
                    {
                        result[pDest++] = ch;
                    }
                }
                return new string(result);
            }
            hasTabs = false;
            return line;
        }

        public static string ConvertTabs(string line, int tabWidth)
        {
            return ConvertTabsEx(line, tabWidth, out _);
        }

        public static bool IsPowerOfTwo(int tabWidth)
        {
            return (tabWidth & (tabWidth - 1)) == 0;
        }

        public static Func<string, int, string> GetBestConvertTabsProc(int tabWidth)
        {
            if (tabWidth < 2) return ConvertTabs1;
            if (IsPowerOfTwo(tabWidth)) return ConvertTabs2n;
            return ConvertTabs;
        }

        public static Func<string, int, bool, string> GetBestConvertTabsProcEx(int tabWidth)
        {
            if (tabWidth < 2) return ConvertTabs1Ex;
            if (IsPowerOfTwo(tabWidth)) return ConvertTabs2nEx;
            return ConvertTabsEx;
        }

        public static int GetExpandedLength(string aStr, int aTabWidth)
        {
            var result = 0;
            foreach (var ch in aStr)
            {
                if (ch == '\t')
                    result += aTabWidth - (result % aTabWidth);
                else
                    result++;
            }
            return result;
        }

        public static int CharIndex2CaretPos(int index, int tabWidth, string line)
        {
            if (index > 1)
            {
                if (tabWidth <= 1 || !GetHasTabs(line.ToCharArray(), out int iChar))
                {
                    return index;
                }

                if (iChar + 1 >= index)
                {
                    return index;
                }
                else
                {
                    var result = iChar;
                    index -= iChar + 1;
                    var pNext = line.Substring(iChar).ToCharArray();
                    foreach (var ch in pNext)
                    {
                        if (index <= 0) break;
                        if (ch == '\t')
                        {
                            result += tabWidth - (result % tabWidth);
                        }
                        else
                        {
                            result++;
                        }
                        index--;
                    }
                    return result + 1;
                }
            }
            return 1;
        }

        public static int CaretPos2CharIndex(int position, int tabWidth, string line, out bool insideTabChar)
        {
            insideTabChar = false;
            if (position > 1)
            {
                if (tabWidth <= 1 || !GetHasTabs(line.ToCharArray(), out int iPos))
                {
                    return position;
                }

                if (iPos + 1 >= position)
                {
                    return position;
                }
                else
                {
                    var result = iPos + 1;
                    var pNext = line.Substring(result).ToCharArray();
                    position--;

                    foreach (var ch in pNext)
                    {
                        if (iPos >= position) break;
                        if (ch == '\t')
                        {
                            iPos += tabWidth;
                            iPos -= iPos % tabWidth;
                            if (iPos > position)
                            {
                                insideTabChar = true;
                                break;
                            }
                        }
                        else
                        {
                            iPos++;
                        }
                        result++;
                    }
                    return result;
                }
            }
            return position;
        }

        public static int StrScanForCharInCategory(string line, int start, Func<char, bool> isOfCategory)
        {
            if (start > 0 && start <= line.Length)
            {
                for (var i = start - 1; i < line.Length; i++)
                {
                    if (isOfCategory(line[i]))
                    {
                        return i + 1;
                    }
                }
            }
            return 0;
        }

        public static int StrRScanForCharInCategory(string line, int start, Func<char, bool> isOfCategory)
        {
            if (start > 0 && start <= line.Length)
            {
                for (var i = start - 1; i >= 0; i--)
                {
                    if (isOfCategory(line[i]))
                    {
                        return i + 1;
                    }
                }
            }
            return 0;
        }

        public static int GetEOL(string line)
        {
            var eol = line.IndexOfAny(new[] { '\0', '\n', '\r' });
            return eol == -1 ? line.Length : eol;
        }

        public static string EncodeString(string s)
        {
            var result = s.Replace("\\", "\\\\").Replace("/", "\\.");
            return result;
        }

        public static string DecodeString(string s)
        {
            var result = s.Replace("\\.", "/").Replace("\\\\", "\\");
            return result;
        }

        public static void FreeAndNil<T>(ref T obj) where T : class
        {
            obj = null;
        }

        public static void Assert(bool expr)
        {
            if (!expr)
            {
                throw new Exception("Assertion failed");
            }
        }

        public static int LastDelimiter(string delimiters, string s)
        {
            for (var i = s.Length - 1; i >= 0; i--)
            {
                if (delimiters.IndexOf(s[i]) >= 0)
                {
                    return i + 1;
                }
            }
            return 0;
        }

        public static string StringReplace(string s, string oldPattern, string newPattern, bool ignoreCase, bool replaceAll)
        {
            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (replaceAll)
            {
                return s.Replace(oldPattern, newPattern, comparison);
            }
            else
            {
                var index = s.IndexOf(oldPattern, comparison);
                if (index < 0) return s;
                return s.Substring(0, index) + newPattern + s.Substring(index + oldPattern.Length);
            }
        }

        public static string DeleteTypePrefixAndSynSuffix(string s)
        {
            if (s.StartsWith("T", StringComparison.OrdinalIgnoreCase))
            {
                s = s.Substring(s.StartsWith("tsyn", StringComparison.OrdinalIgnoreCase) ? 4 : 1);
            }
            if (s.EndsWith("syn", StringComparison.OrdinalIgnoreCase))
            {
                s = s.Substring(0, s.Length - 3);
            }
            return s;
        }

        public static bool EnumHighlighterAttris(TSynCustomHighlighter highlighter, bool skipDuplicates, Func<TSynCustomHighlighter, TSynHighlighterAttributes, string, object[], bool> highlighterAttriProc, params object[] parameters)
        {
            if (highlighter == null || highlighterAttriProc == null) return false;

            var highlighterList = new System.Collections.Generic.List<TSynCustomHighlighter>();
            return InternalEnumHighlighterAttris(highlighter, skipDuplicates, highlighterAttriProc, parameters, highlighterList);
        }

        private static bool InternalEnumHighlighterAttris(TSynCustomHighlighter highlighter, bool skipDuplicates, Func<TSynCustomHighlighter, TSynHighlighterAttributes, string, object[], bool> highlighterAttriProc, object[] parameters, System.Collections.Generic.List<TSynCustomHighlighter> highlighterList)
        {
            if (highlighterList.Contains(highlighter))
            {
                if (skipDuplicates) return true;
            }
            else
            {
                highlighterList.Add(highlighter);
            }

            if (highlighter is TSynMultiSyn multiSyn)
            {
                if (!InternalEnumHighlighterAttris(multiSyn.DefaultHighlighter, skipDuplicates, highlighterAttriProc, parameters, highlighterList)) return false;

                for (var i = 0; i < multiSyn.Schemes.Count; i++)
                {
                    var scheme = multiSyn.Schemes[i];
                    var uniqueAttriName = $"{highlighter.ExportName}{GetHighlighterIndex(highlighter, highlighterList)}.{scheme.MarkerAttri.Name}{i + 1}";

                    if (!highlighterAttriProc(highlighter, scheme.MarkerAttri, uniqueAttriName, parameters)) return false;

                    if (!InternalEnumHighlighterAttris(scheme.Highlighter, skipDuplicates, highlighterAttriProc, parameters, highlighterList)) return false;
                }
            }
            else if (highlighter != null)
            {
                for (var i = 0; i < highlighter.AttrCount; i++)
                {
                    var uniqueAttriName = $"{highlighter.ExportName}{GetHighlighterIndex(highlighter, highlighterList)}.{highlighter.Attribute[i].Name}";

                    if (!highlighterAttriProc(highlighter, highlighter.Attribute[i], uniqueAttriName, parameters)) return false;
                }
            }

            return true;
        }

        private static int GetHighlighterIndex(TSynCustomHighlighter highlighter, System.Collections.Generic.List<TSynCustomHighlighter> highlighterList)
        {
            var count = 1;
            foreach (var item in highlighterList)
            {
                if (item == highlighter) return count;
                if (item?.GetType() == highlighter?.GetType()) count++;
            }
            return count;
        }

        public static readonly ushort[] fcstab = new ushort[]
        {
            0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
            0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
            0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
            0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
            0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
            0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
            0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
            0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
            0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
            0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
            0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
            0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
            0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
            0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
            0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
            0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
            0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
            0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
            0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
            0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
            0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
            0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
            0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
            0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
            0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
            0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
            0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
            0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
            0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
            0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
            0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
            0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78
        };

        public static ushort CalcFCS(byte[] buffer, uint bufSize)
        {
            ushort curFCS = 0xffff;
            for (int i = 0; i < bufSize; i++)
            {
                curFCS = (ushort)((curFCS >> 8) ^ fcstab[(curFCS ^ buffer[i]) & 0xff]);
            }
            return curFCS;
        }

        public static void SynDrawGradient(Graphics canvas, Color startColor, Color endColor, int steps, Rectangle rect, bool horizontal)
        {
            var startColorR = startColor.R;
            var startColorG = startColor.G;
            var startColorB = startColor.B;

            var diffColorR = endColor.R - startColorR;
            var diffColorG = endColor.G - startColorG;
            var diffColorB = endColor.B - startColorB;

            steps = MinMax(steps, 2, 256);

            if (horizontal)
            {
                var size = rect.Width;
                var paintRect = new Rectangle(rect.X, rect.Y, 0, rect.Height);

                for (var i = 0; i < steps; i++)
                {
                    paintRect.X = rect.X + (i * size) / steps;
                    paintRect.Width = (i + 1) * size / steps - paintRect.X;
                    canvas.FillRectangle(new SolidBrush(Color.FromArgb(startColorR + (i * diffColorR) / (steps - 1),
                                                                      startColorG + (i * diffColorG) / (steps - 1),
                                                                      startColorB + (i * diffColorB) / (steps - 1))), paintRect);
                }
            }
            else
            {
                var size = rect.Height;
                var paintRect = new Rectangle(rect.X, rect.Y, rect.Width, 0);

                for (var i = 0; i < steps; i++)
                {
                    paintRect.Y = rect.Y + (i * size) / steps;
                    paintRect.Height = (i + 1) * size / steps - paintRect.Y;
                    canvas.FillRectangle(new SolidBrush(Color.FromArgb(startColorR + (i * diffColorR) / (steps - 1),
                                                                      startColorG + (i * diffColorG) / (steps - 1),
                                                                      startColorB + (i * diffColorB) / (steps - 1))), paintRect);
                }
            }
        }
    }

    // Assuming these types are placeholder implementations for missing types in the provided Delphi code.
    public class TSynCustomHighlighter
    {
        public virtual int AttrCount => 0;
        public virtual string ExportName => "SynCustomHighlighter";
        public virtual TSynHighlighterAttributes Attribute[int index] => null;
    }

    public class TSynHighlighterAttributes
    {
        public string Name { get; set; }
    }

    public class TSynMultiSyn : TSynCustomHighlighter
    {
        public TSynCustomHighlighter DefaultHighlighter { get; set; }
        public System.Collections.Generic.List<Scheme> Schemes { get; set; } = new System.Collections.Generic.List<Scheme>();

        public class Scheme
        {
            public TSynHighlighterAttributes MarkerAttri { get; set; }
            public TSynCustomHighlighter Highlighter { get; set; }
        }
    }
}
