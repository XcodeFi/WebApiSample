using ShCore.Utility;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Text;
using System.Collections.Generic;

namespace ShCore.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Format một String
        /// </summary>
        /// <param name="str"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public static string Frmat(this string str, params object[] @params)
        {
            return string.Format(str, @params);
        }

        /// <summary>
        /// Kiểm tra xem string có Null hay không
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNull(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Kiểm tra string nó null hay ko
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNull(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// swith giá trị khi mà str là Empty
        /// </summary>
        /// <param name="str"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string WhenEmpty(this string str, Func<string> action)
        {
            return str.IsNull() ? action() : str;
        }

        /// <summary>
        /// Mã hóa
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encryt(this string str)
        {
            var encrypt = CenterSecurity.Inst.EnCrypt(str);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encrypt.T1 + "@" + encrypt.T2));
        }

        /// <summary>
        /// Giải mã 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decrypt(this string str)
        {
            str = Encoding.UTF8.GetString(Convert.FromBase64String(str));
            var data = str.Split('@');
            return data.Length == 2 ? CenterSecurity.Inst.Decrypt(data[0], data[1]) : string.Empty;
        }

        /// <summary>
        /// Ma hoa mat khau
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptPassword(this string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        /// <summary>
        /// StripHTML
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string StripHTML(this string htmlString)
        {
            string pattern = @"<(.|\n)*?>";
            return Regex.Replace(htmlString, pattern, string.Empty);
        }

        /// <summary>
        /// Sub một string lấy ra totalWord
        /// </summary>
        /// <param name="words"></param>
        /// <param name="totalWord"></param>
        /// <returns></returns>
        public static string SubWord(this string words, int totalWord)
        {
            return string.Join(" ", words.Split(' ').Take(totalWord).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstToUpper(this string input)
        {
            input = input.ToLower();
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        const string uniChars = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";
        const string KoDauChars = "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";
        /// <summary>
        /// Tạo chuỗi tiếng việt không dấu
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UnicodeFormat(this string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            string retVal = string.Empty;
            int pos;
            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += KoDauChars[pos];
                else
                    retVal += s[i];
            }
            string temp = retVal;
            for (int i = 0; i < retVal.Length; i++)
            {
                pos = Convert.ToInt32(retVal[i]);
                if (!((pos >= 97 && pos <= 122) || (pos >= 65 && pos <= 90) || (pos >= 48 && pos <= 57) || pos == 32))
                    temp = temp.Replace(retVal[i].ToString(), "");
            }
            temp = temp.Replace(" ", "-");
            while (temp.EndsWith("-"))
                temp = temp.Substring(0, temp.Length - 1);

            while (temp.IndexOf("--") >= 0)
                temp = temp.Replace("--", "-");

            retVal = temp;
            retVal = retVal.Replace("\"", string.Empty);
            retVal = retVal.Replace("'", string.Empty);
            return retVal.ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static IEnumerable<Pair<string,int>> DoSplit(this string s, char c)
        {
            return s.Split(c).Select((item, i) => new Pair<string, int> { T1 = item, T2 = i });
        }
    }
}
