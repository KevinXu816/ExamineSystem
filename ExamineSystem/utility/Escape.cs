using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ExamineSystem.utility
{
    public static class Escape
    {
        // Fields
        private static string[] hex = new string[] { 
        "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F", 
        "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F", 
        "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F", 
        "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F", 
        "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F", 
        "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F", 
        "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F", 
        "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F", 
        "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F", 
        "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F", 
        "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF", 
        "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF", 
        "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF", 
        "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF", 
        "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF", 
        "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF"
     };
        private static Random random = new Random();
        private static byte[] val = new byte[] { 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 10, 11, 12, 13, 14, 15, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 10, 11, 12, 13, 14, 15, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
        0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f
     };

        // Methods
        public static string JsEscape(string s)
        {
            StringBuilder builder = new StringBuilder();
            int length = s.Length;
            for (int i = 0; i < length; i++)
            {
                int index = s[i];
                if ((0x41 <= index) && (index <= 90))
                {
                    builder.Append((char)index);
                }
                else if ((0x61 <= index) && (index <= 0x7a))
                {
                    builder.Append((char)index);
                }
                else if ((0x30 <= index) && (index <= 0x39))
                {
                    builder.Append((char)index);
                }
                else if ((((index == 0x2d) || (index == 0x5f)) || ((index == 0x2e) || (index == 0x2a))) || (((index == 0x2b) || (index == 0x2f)) || (index == 0x40)))
                {
                    builder.Append((char)index);
                }
                else if (index <= 0x7f)
                {
                    builder.Append('%');
                    builder.Append(hex[index]);
                }
                else
                {
                    builder.Append('%');
                    builder.Append('u');
                    builder.Append(hex[index >> 8]);
                    builder.Append(hex[0xff & index]);
                }
            }
            return builder.ToString();
        }

        public static string RegEscape(string str)
        {
            return Regex.Escape(str);
        }

        public static string JsUnEscape(string s)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            int length = s.Length;
            while (num < length)
            {
                int num3 = s[num];
                if ((0x41 <= num3) && (num3 <= 90))
                {
                    builder.Append((char)num3);
                }
                else if ((0x61 <= num3) && (num3 <= 0x7a))
                {
                    builder.Append((char)num3);
                }
                else if ((0x30 <= num3) && (num3 <= 0x39))
                {
                    builder.Append((char)num3);
                }
                else if ((((num3 == 0x2d) || (num3 == 0x5f)) || ((num3 == 0x2e) || (num3 == 0x2a))) || (((num3 == 0x2b) || (num3 == 0x2f)) || (num3 == 0x40)))
                {
                    builder.Append((char)num3);
                }
                else if (num3 == 0x25)
                {
                    int num4 = 0;
                    if ('u' != s[num + 1])
                    {
                        num4 = (num4 << 4) | val[s[num + 1]];
                        num4 = (num4 << 4) | val[s[num + 2]];
                        num += 2;
                    }
                    else
                    {
                        num4 = (num4 << 4) | val[s[num + 2]];
                        num4 = (num4 << 4) | val[s[num + 3]];
                        num4 = (num4 << 4) | val[s[num + 4]];
                        num4 = (num4 << 4) | val[s[num + 5]];
                        num += 5;
                    }
                    builder.Append((char)num4);
                }
                num++;
            }
            return builder.ToString();
        }

        public static string RegUnEscape(string str)
        {
            return Regex.Unescape(str);
        }

    }

}
