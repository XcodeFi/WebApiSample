using System;
using ShCore.Attributes;
using ShCore.Reflectors;
using System.Reflection;
using ShCore.Utility;
using System.Collections.Generic;
namespace ShCore.Extensions
{
    /// <summary>
    /// Phương thức mở rộng cho Type
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Lấy ra danh sách PropertyInfo của một type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetListProperties(this Type type)
        { 
            return new ReflectTypeListProperty()[type];
        }

        /// <summary>
        /// Lấy ra thuộc tính mà Public
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertiesPublic(this Type type)
        {
            return new ReflectTypeListPropertyPublic()[type];
        }

        /// <summary>
        /// Lấy ra Attribute ClassInfoAttribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TAttribute GetClassInfoAttribute<TAttribute>(this Type type) where TAttribute : ClassInfoAttribute
        {
            return new ReflectClassInfo<TAttribute>()[type];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Pair<PropertyInfo, TAttribute>> GetListPairPropertyAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return new ReflectTypeListPropertyWithAttribute<TAttribute>()[type];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Pair<PropertyInfo, List<TAttribute>>> GetListPairPropertyListAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return new ReflectTypeListPropertyWithListAttribute<TAttribute>()[type];
        }

        /// <summary>
        /// Khởi tạo một đối tượng theo Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type type, params object[] param)
        {
            return Activator.CreateInstance(type, param);
        }

        /// <summary>
        /// Khởi tạo một đối tượng theo Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(this Type type, params object[] param)
        {
            return (T)type.CreateInstance(param);
        }
    }
}
