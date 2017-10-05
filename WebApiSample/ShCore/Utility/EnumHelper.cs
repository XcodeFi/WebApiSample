using System;
using System.Linq;
using System.Collections.Generic;
using ShCore.Attributes;
using ShCore.Reflectors;
using ShCore.Extensions;
namespace ShCore.Utility
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumHelper<T> : Singleton<EnumHelper<T>>
    {
        private Type type = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public EnumHelper() { type = typeof(T); }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public List<TAttribute> GetAttributes<TAttribute>() where TAttribute : FieldInfoAttribute
        {
            return new ReflectFieldInfo<TAttribute>()[type];
        }

        /// <summary>
        /// Convert từ string sang Enum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public T ParseFromString(string value, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TAttribute"></typeparam>
    public class EnumHelper<T, TAttribute> : Singleton<EnumHelper<T, TAttribute>> where TAttribute : FieldInfoAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<object, TAttribute> dic = null;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<object, TAttribute> Dic
        {
            get 
            {
                if (dic == null)
                    dic = EnumHelper<T>.Inst.GetAttributes<TAttribute>().ToDictionary(f => f.FieldValue, f => f);
                return dic; 
            }
        }

        /// <summary>
        /// GetAttribute
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public TAttribute GetAttribute(T t)
        {
            return Dic.TryGet(t);
        }
    }
}
