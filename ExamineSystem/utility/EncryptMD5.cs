using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System;

namespace ExamineSystem.utility
{
    public static class EncryptMD5
    {
        public static string MD5toFileCode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(text, "md5");
        }

        public static string MD5to16Code(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string code = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(text)), 4, 8);
            code = code.Replace("-", "");
            return code;
        }

        public static string MD5to32Code(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            StringBuilder code = new StringBuilder();
            byte[] buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
            for (int i = 0; i < buffer.Length; i++)
            {
                code.Append(buffer[i].ToString("X"));
            }
            return code.ToString();
        }

    }
}