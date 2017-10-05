using System;
using System.Reflection;
using System.Collections.Generic;
namespace ShCore.Extender
{
    /// <summary>
    /// Phương thức mở rộng cho MemberInfo
    /// </summary>
    public static class MemberInfoExtender
    {
        /// <summary>
        /// Lấy thông tin Attribute của MemberInfo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mif"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this MemberInfo mif) where T : Attribute
        {
            return MemberInfo<T>.Inst[mif];
        }

        ///// <summary>
        ///// Lấy ra list attribute của một member
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="mif"></param>
        ///// <returns></returns>
        //public static List<T> GetListAttribute<T>(this MemberInfo mif) where T : Attribute
        //{
        //    return MemberInfoList<T>.Inst[mif];
        //}

        /// <summary>
        /// Kiểm tra xem MemberInfo có Attribute T hay không
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mif"></param>
        /// <returns></returns>
        public static bool HasAttribute<T>(this MemberInfo mif) where T : Attribute
        {
            return mif.GetCustomAttributes(typeof(T), true).Length != 0;
        }
    }
}
