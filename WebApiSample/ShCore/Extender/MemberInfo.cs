
using System.Reflection;
using System;
namespace ShCore.Extender
{
    /// <summary>
    /// Thông tin một member info với attribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MemberInfo<T> : DictionaryCacheBase<MemberInfo, T, MemberInfo<T>> where T : Attribute
    {
        /// <summary>
        /// Lấy ra Attribute của một MemberInfo
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override T GetValueForDic(MemberInfo key)
        {
            // Lấy ra Attributes
            object[] attr = key.GetCustomAttributes(typeof(T), true);

            // return kết quả
            return attr.Length > 0 ? attr[0].As<T>() : null;
        }
    }
}
