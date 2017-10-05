
using System;
namespace ShCore.Extender
{
    /// <summary>
    /// Function Extender cho đối tượng
    /// </summary>
    public static class ObjextExtender
    {
        /// <summary>
        ///  Lấy ra Attribute của một đối tượng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this object obj) where T : Attribute { return obj.GetType().GetAttribute<T>(); }

        /// <summary>
        /// Cast object to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T As<T>(this object obj) { return obj.IsNull() ? default(T) : (T)obj; }
        /// <summary>
        /// Kiểm tra xem một đối tượng có Null hay không
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj) { return obj == null || obj.Equals(DBNull.Value); }
    }
}
