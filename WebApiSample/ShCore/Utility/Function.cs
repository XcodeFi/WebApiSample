using System;
using System.Text;
namespace ShCore.Utility
{
    /// <summary>
    /// Thư viện 
    /// </summary>
    public class Function : Singleton<Function>
    {
        private string abc = "abcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// Đối tượng tạo random
        /// </summary>
        private Random rnd = new Random();

        /// <summary>
        /// Tạo chuỗi Random
        /// </summary>
        /// <returns></returns>
        public string Random(int length)
        {
            
            var ret = new StringBuilder();
            for (int i = 0; i < length; i++)
                ret.Append(abc.Substring(rnd.Next(abc.Length), 1));
            return ret.ToString();
        }
    }
}
