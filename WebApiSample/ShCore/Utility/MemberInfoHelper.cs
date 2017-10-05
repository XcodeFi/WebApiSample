using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;
namespace ShCore.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class MemberInfoHelper : Singleton<MemberInfoHelper>
    {
        /// <summary>
        /// Lấy Attribute của một MemberInfo
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="mi"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public TAttribute GetAttribute<TAttribute>(MemberInfo mi, bool inherit) where TAttribute : Attribute
        {
            var arr = mi.GetCustomAttributes(typeof(TAttribute), inherit);
            return arr.Length == 0 ? null : arr[0] as TAttribute;
        }

        /// <summary>
        /// Lấy ra danh sách Attribute của một MemberInfo
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="mi"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public List<TAttribute> GetAttributes<TAttribute>(MemberInfo mi, bool inherit) where TAttribute : Attribute
        {
            return mi.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().ToList();
        }

        /// <summary>
        /// Lấy ra danh sách cặp đôi property và attribute của một Type
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public List<Pair<PropertyInfo, TAttribute>> GetListPairPropertyAttribute<TAttribute>(Type type, bool inherit) where TAttribute : Attribute
        {
            return type.GetProperties().Select(p => new Pair<PropertyInfo, TAttribute> { T1 = p, T2 = GetAttribute<TAttribute>(p, inherit) }).Where(pr => pr.T2 != null).ToList();
        }

        /// <summary>
        /// Lấy ra danh sách cặp đôi property và List Attribute của một Type
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public List<Pair<PropertyInfo, List<TAttribute>>> GetListPairPropertyListAttribute<TAttribute>(Type type, bool inherit) where TAttribute : Attribute
        {
            return type.GetProperties().Select(p => new Pair<PropertyInfo, List<TAttribute>> { T1 = p, T2 = GetAttributes<TAttribute>(p, inherit) }).Where(pr => pr.T2.Count != 0).ToList();
        }
    }
}
