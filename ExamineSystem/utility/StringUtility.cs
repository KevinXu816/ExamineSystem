using System;
using System.Collections.Generic;
using System.Web;

namespace ExamineSystem.utility
{
    public static class StringUtility
    {

        public static string[] Split(string text, string separator)
        {
            string[] result = text.Split(new string[] { separator }, StringSplitOptions.None);
            return result;
        }

        public static bool ConvertBool(string text)
        {
            text = (text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(text))
                return false;
            bool resutl = false;
            if (!bool.TryParse(text, out resutl))
            {
                if (text[0] == '1')
                    resutl = true;
                else
                    resutl = false;
            }
            return resutl;
        }

        public static int[] SplitStringToIntArray(string text, string separator)
        {
            text = (text ?? string.Empty).Trim();
            string[] strCol = Split(text, separator);
            List<int> intLis = new List<int>();
            foreach (string strItm in strCol)
            {
                string tmpStr = (strItm ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(tmpStr))
                    continue;
                else
                {
                    int tmpInt = 0;
                    if (int.TryParse(tmpStr, out tmpInt))
                        intLis.Add(tmpInt);
                    else
                        continue;
                }
            }
            return intLis.ToArray();
        }

        public static string ConvertAlphabet(int num)
        {
            num = num < 0 ? 0 : (num > 25 ? 25 : num);
            int index = 65 + num;
            char alphabet = (char)index;
            return alphabet.ToString();
        }

    }
}