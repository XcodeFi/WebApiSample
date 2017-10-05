using System.IO;
using System.Web;
using ShCore.Extensions;
using System;
namespace ShCore.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class DiskDrive : Singleton<DiskDrive>
    {
        /// <summary>
        /// Đọc nội dung file ra string
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string LoadFile(string filePath)
        {   
            // Mở file để đọc
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {                
                using(var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Thực hiện Load File
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="action"></param>
        public void LoadFile(string filePath, Action<string, int> action)
        {   
            // Mở file để đọc
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                // tạo reader để đọc file
                using (var reader = new StreamReader(stream))
                {
                    // đọc từng dòng
                    int start = 1;
                    while (!reader.EndOfStream)
                    {
                        action(reader.ReadLine(), start);
                        start++;
                    }
                }
            }            
        }
    }

    /// <summary>
    /// Mở rộng phương thức cho string
    /// </summary>
    public static class StringExtender
    {
        /// <summary>
        /// Server.MapPath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ServerMapPath(this string filePath)
        {
            return HttpContext.Current.Server.MapPath("~{0}".Frmat(filePath));
        }

        /// <summary>
        /// Lấy ra nội dung file, MapPath trên web
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileContent(this string filePath)
        {
            return DiskDrive.Inst.LoadFile(filePath.ServerMapPath());
        }
    }
}
